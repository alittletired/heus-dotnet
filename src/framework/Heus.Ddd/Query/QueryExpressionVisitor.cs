using System.Linq.Expressions;
using System.Reflection;
using Heus.Ddd.Dtos;
namespace Heus.Ddd.Query;
internal class QueryExpressionVisitor<T> : ExpressionVisitor
{

    private readonly FilterMapping _filterMapping;
    private readonly IQueryable _queryable;
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo WhereMethod = typeof(Queryable).GetRuntimeMethods()
        .First(s => s.Name == nameof(Queryable.Where) && s.GetParameters().Length == 2);

    private IPageRequest<T>? _pageRequest;
    private Expression? _selectExpression;
    public QueryExpressionVisitor(IQueryable queryable)
    {
        _queryable= queryable;
        var genericArguments = _queryable.ElementType.GetGenericArguments();
        _filterMapping = QueryFilterHelper.GetDynamicMappings(typeof(T), genericArguments);
    }

    protected override Expression VisitMethodCall(MethodCallExpression methodCall)
    {
        if (methodCall.Method.Name == "SelectMany" && _selectExpression==null)
        {
            var obj = Visit(methodCall.Object);
            var paras = methodCall.Arguments.ToArray();
            paras[^1] = TranslateSelect((UnaryExpression)paras[^1]);
            paras = paras.Select(Visit).ToArray()!;
            var genericTypes = methodCall.Method.GetGenericArguments().ToArray();
            genericTypes[^1] = typeof(T);
            var selectMany = methodCall.Method.GetGenericMethodDefinition().MakeGenericMethod(genericTypes);
            _selectExpression = Expression.Call(obj, selectMany, paras);
            return _selectExpression;
        }

        return base.VisitMethodCall(methodCall);
    }

    public IQueryable<T> Translate(IPageRequest<T>? pageRequest)
    {
        _pageRequest = pageRequest;
        _selectExpression = null;
        //如果类型相同，并且没有过滤条件，则直接返回
        if (_pageRequest==null && typeof(T) == _queryable.ElementType)
            return (IQueryable<T>)_queryable;
        var expr = Visit(_queryable.Expression);
        return _queryable.Provider.CreateQuery<T>(expr);

    }

    protected override Expression VisitExtension(Expression node)
    {
        if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(IQueryable<>))
        {
            var type = node.Type.GenericTypeArguments[0];
            var filterExpression = GetFilterExpression(type);
            if (filterExpression == null)
            {
                return base.VisitExtension(node);
            }

            var whereMethod = WhereMethod.MakeGenericMethod(type);
            return Expression.Call(null, whereMethod, node, filterExpression);
        }

        return base.VisitExtension(node);
    }

    private Expression TranslateSelect(UnaryExpression expression)
    {
        var lambda = (LambdaExpression)expression.Operand;
       
        var parameters = lambda.Parameters.Select(p => (ParameterExpression)Visit(p)).ToList();
        var express = GetSelectExpression(_filterMapping, parameters);
        var newLambda = Expression.Lambda(express, parameters);
        return Expression.Quote(newLambda);
    }

    private static Expression GetSelectExpression(FilterMapping mapping, IEnumerable<ParameterExpression> parameters)
    {
        var memberBindings = new List<MemberBinding>();
        var parameterList = parameters.ToList();
        foreach (var mappingItem in mapping.Mappings.Values)
        {
            var dtoProp = mappingItem.DtoProperty;
            var entityProp = mappingItem.EntityProperty;
            Expression parameter = parameterList[mappingItem.ParamIndex];
            //left join 会导致参数表达式包裹一层，故需要再次取出
            if(parameter.Type!= mappingItem.EntityType)
            {
                var prop = parameter.Type.GetProperties().First(s => s.PropertyType == mappingItem.EntityType);
                parameter = Expression.MakeMemberAccess(parameter, prop);
            
            }
            var expression = Expression.MakeMemberAccess(parameter, entityProp);
            if (dtoProp.PropertyType != entityProp.PropertyType)
            {
                var covertExpr = Expression.Convert(expression, dtoProp.PropertyType);
                memberBindings.Add(Expression.Bind(dtoProp, covertExpr));
            }
            else
            {
                memberBindings.Add(Expression.Bind(dtoProp, expression));
            }

        }

        return Expression.MemberInit(Expression.New(mapping.DtoType), memberBindings);
    }

    private Expression GetFilterItemExpression(MemberExpression memberExpr
        ,string propName, DynamicSearchFilter filter)
    {
        Expression filterExpr=null! ;
        var propertyInfo =(PropertyInfo)memberExpr.Member ;
        var value = filter.Value.ToString()!;
        Expression constExpr;
        switch (filter.Op)
        {
            case OperatorTypes.Equal:
                constExpr= Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
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
                var startsWith = typeof(string).GetMethod(nameof(string.StartsWith), new [] { typeof(string) })!;
                filterExpr = Expression.Call(memberExpr, startsWith, constExpr);
                break;
            case OperatorTypes.TailLike:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                var endsWith = typeof(string).GetMethod(nameof(string.EndsWith), new [] { typeof(string) })!;
                filterExpr = Expression.Call(memberExpr, endsWith, constExpr);
                break;
            case OperatorTypes.Like:
                constExpr = Expression.Constant(ChangeType(value, propertyInfo.PropertyType));
                var contains = typeof(string).GetMethod(nameof(string.Contains), new [] { typeof(string) })!;
                filterExpr = Expression.Call(memberExpr, contains, constExpr);
                break;
            case OperatorTypes.In:
            case OperatorTypes.NotIn:
                constExpr = Expression.Constant(value);
                filterExpr = Expression.Call(typeof(Enumerable), "Contains", new[] { propertyInfo.PropertyType }, constExpr, memberExpr);
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
          return  Convert.ChangeType(val,type);
        }

        return val;
    }

    private Expression? GetFilterExpression(Type entityType)
    {
        var dynamicQuery = _pageRequest as DynamicSearch<T>;
        if (dynamicQuery == null)
            return null;
      
        var paramExpr = Expression.Parameter(entityType, "p");
        var filters = new List<Expression>();
        foreach (var (propName, filter) in dynamicQuery.Filters)
        {

            if (!_filterMapping.Mappings.TryGetValue(propName, out var mappingItem))
            {
                continue;
            }
            if (mappingItem.EntityType != entityType)
            {
                continue;
            }

            var memberExpr = Expression.Property(paramExpr, mappingItem.EntityProperty);
            var filterExpr= GetFilterItemExpression(memberExpr, propName, filter);
            filters.Add(filterExpr);

        }

        if (filters.Count == 0)
            return null;
        var finalExpr = filters[0];
        for (var i = 1; i < filters.Count; i++)
        {
            finalExpr = Expression.AndAlso(finalExpr, filters[i]);
        }

        return Expression.Lambda(finalExpr, paramExpr);

    }

}
