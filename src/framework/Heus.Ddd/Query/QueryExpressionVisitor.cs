using System.Linq.Expressions;
using System.Reflection;
using Heus.Core.Utils;
using Heus.Ddd.Dtos;

namespace Heus.Ddd.Query;
internal class QueryExpressionVisitor<TSource, TDto> : ExpressionVisitor
{
// ReSharper disable once StaticMemberInGenericType
  

    private readonly FilterMapping _filterMapping;
    private readonly IQueryable<TSource> _queryable;
    private readonly IPageRequest<TDto>? _pageRequest;
    public QueryExpressionVisitor(IQueryable<TSource> queryable, IPageRequest<TDto>? pageRequest)
    {

        _queryable = queryable;
        _pageRequest = pageRequest;
        var elementType = _queryable.ElementType;
        _filterMapping = QueryFilterHelper.GetDynamicMappings(typeof(TDto), elementType);
    }
   

    public IQueryable<TDto> Translate()
    {
        //如果类型相同，并且没有过滤条件，则直接返回
        if (_pageRequest == null && typeof(TDto) == _queryable.ElementType)
            return (IQueryable<TDto>)_queryable;
        var expr =Visit(_queryable.Expression);
        expr = ApplyFilter(expr);
        expr = ApplySelect(expr);
        return _queryable.Provider.CreateQuery<TDto>(expr);

    }
    private Expression ApplySelect(Expression expression)
    {
        var returnType = expression.Type.GetGenericArguments()[0];
        if (returnType == typeof(TDto))
        {
            return expression;
        }
        var selectMethods =QueryFilterHelper.SelectMethodInfo
            .MakeGenericMethod(returnType, typeof(TDto));
        var paramExpr = Expression.Parameter(returnType, "p");
        var selectExpr = GetSelectLambda(_filterMapping, paramExpr);
        return  Expression.Call(null, selectMethods, expression, selectExpr);
    }
    private Expression ApplyFilter(Expression expression)
    {
        var type = expression.Type.GetGenericArguments()[0];
        var filterExpr = GetFilterLambda( type);
        if (filterExpr == null)
        {
            return expression;
        }
        var whereMethod =QueryFilterHelper.WhereMethodInfo.MakeGenericMethod(type);
        return Expression.Call(null, whereMethod, expression, filterExpr);
    }


    private static MemberExpression GetMemberExpression( MappingItem mappingItem, params ParameterExpression[] parameters)
    {

        foreach (var parameter in parameters)
        {
            if (TryGetProperty(parameter.Type, mappingItem, out var props))
            {
                var memberExpr = Expression.MakeMemberAccess(parameter, props[0]);
                for (var i = 1; i < props.Count; i++)
                {
                    memberExpr = Expression.MakeMemberAccess(memberExpr, props[i]);
                }
                return memberExpr;
            }
        }
        throw new InvalidOperationException($"无法找到成员，请检查查询，Entity:{mappingItem.EntityType.Name},EntityPropertyName:{mappingItem.EntityProperty.Name},DtoPropertyName:{mappingItem.DtoProperty.Name}");
    }
    private static  bool TryGetProperty(Type type, MappingItem mappingItem, out List<PropertyInfo> props)
    {
        props= new List<PropertyInfo>();
        if (type== mappingItem.EntityType)
        {
            var prop = type.GetProperties().First(s => s.Name == mappingItem.EntityProperty.Name);
            props.Add(prop);
         
        }
        else if (TypeUtils.IsAnonymousType(type))
        {
           foreach(var prop in type.GetProperties())
            {
                if(TryGetProperty(prop.PropertyType, mappingItem,out var subProps))
                {
                    props.Add(prop);
                    props.AddRange(subProps);
                    break;
                }
            }  
        }
        return props.Count>0;
    }
    private static LambdaExpression GetSelectLambda(FilterMapping mapping, params ParameterExpression[] parameters)
    {
        var memberBindings = new List<MemberBinding>();
        foreach (var mappingItem in mapping.Mappings.Values)
        {
            var dtoProp = mappingItem.DtoProperty;
            var entityProp = mappingItem.EntityProperty;

            var memberExpr = GetMemberExpression(mappingItem, parameters);
            if (dtoProp.PropertyType != entityProp.PropertyType)
            {
                var covertExpr = Expression.Convert(memberExpr, dtoProp.PropertyType);
                memberBindings.Add(Expression.Bind(dtoProp, covertExpr));
            }
            else
            {
                memberBindings.Add(Expression.Bind(dtoProp, memberExpr));
            }
        }
        var memberInitExpr = Expression.MemberInit(Expression.New(mapping.DtoType), memberBindings);
        return Expression.Lambda(memberInitExpr, parameters);
    }

    private Expression GetFilterItemExpression(MemberExpression memberExpr
        , string propName, DynamicSearchFilter filter)
    {
        Expression filterExpr = null!;
        var propertyInfo = (PropertyInfo)memberExpr.Member;
        var value = filter.Value;
        Expression constExpr;
        switch (filter.Op)
        {
            case OperatorTypes.Equal:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                filterExpr = Expression.Equal(memberExpr, constExpr);
                break;
            case OperatorTypes.GreaterThan:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                filterExpr = Expression.GreaterThan(memberExpr, constExpr);
                break;
            case OperatorTypes.GreaterOrEqual:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                filterExpr = Expression.GreaterThanOrEqual(memberExpr, constExpr);
                break;
            case OperatorTypes.LessThan:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                filterExpr = Expression.LessThan(memberExpr, constExpr);
                break;
            case OperatorTypes.LessOrEqual:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                filterExpr = Expression.LessThanOrEqual(memberExpr, constExpr);
                break;

            case OperatorTypes.HeadLike:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                var startsWith = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!;
                filterExpr = Expression.Call(memberExpr, startsWith, constExpr);
                break;
            case OperatorTypes.TailLike:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                var endsWith = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })!;
                filterExpr = Expression.Call(memberExpr, endsWith, constExpr);
                break;
            case OperatorTypes.Like:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                var contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                filterExpr = Expression.Call(memberExpr, contains, constExpr);
                break;
            case OperatorTypes.In:
            case OperatorTypes.NotIn:
                constExpr = Expression.Constant(value);
                filterExpr = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains) , new[] { propertyInfo.PropertyType }, constExpr, memberExpr);
                if (filter.Op == OperatorTypes.NotIn)
                {
                    filterExpr = Expression.Not(filterExpr);
                }

                break;
        }
        if (filterExpr == null)
        {
            throw new InvalidOperationException($"无法生成查询条件propName:{propName} filter: {filter}");
        }
        return filterExpr;
    }
    private object ChangeType(object val, Type type)
    {
        if (type != typeof(string))
        {
            return Convert.ChangeType(val, type);
        }

        return val;
    }

    private LambdaExpression? GetFilterLambda(Type parameterType)
    {
        var dynamicQuery = _pageRequest as DynamicSearch<TDto>;
        if (dynamicQuery == null)
            return null;

        var paramExpr = Expression.Parameter(parameterType, "p");
        var filters = new List<Expression>();
        foreach (var (propName, filter) in dynamicQuery.Filters)
        {
            if (!_filterMapping.Mappings.TryGetValue(propName, out var mappingItem))
            {
                continue;
            }
     
            var memberExpr = GetMemberExpression(mappingItem, paramExpr);
            var filterExpr = GetFilterItemExpression(memberExpr, propName, filter);
            filters.Add(filterExpr);

        }

        if (filters.Count == 0)
        {
            return null;
        }

        var finalExpr = filters[0];
        for (var i = 1; i < filters.Count; i++)
        {
            finalExpr = Expression.AndAlso(finalExpr, filters[i]);
        }

        return Expression.Lambda(finalExpr, paramExpr);

    }

}
