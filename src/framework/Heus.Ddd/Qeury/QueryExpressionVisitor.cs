
using Heus.Ddd.Dtos;
using System.Diagnostics.CodeAnalysis;

using System.Linq.Expressions;
using System.Reflection;

namespace Heus.Ddd.Qeury;

internal class QueryExpressionVisitor<T> : ExpressionVisitor 
{
    private readonly IQueryDto<T> _queryDto;
    private FilterMapping _filterMapping = null!;

    private static readonly MethodInfo WhereMethod = typeof(Queryable).GetRuntimeMethods()
        .First(s => s.Name == nameof(Queryable.Where) && s.GetParameters().Length == 2);

    public QueryExpressionVisitor(IQueryDto<T> data)
    {
        _queryDto = data;
    }

    [return: NotNullIfNotNull("node")]
    public Expression? VisitStart(Expression? node)
    {
        if (node == null)
            return null;
        if (node is MethodCallExpression methodCall)
        {
            var obj = Visit(methodCall.Object);
            var paras = methodCall.Arguments.ToArray();
            paras[^1] = TranslateSelect((UnaryExpression)paras[^1]!);
            paras = paras.Select(Visit).ToArray()!;
            var genericTypes = methodCall.Method.GetGenericArguments().ToArray();
            genericTypes[^1] = typeof(T);
            var selectMany = methodCall.Method.GetGenericMethodDefinition().MakeGenericMethod(genericTypes);

            return Expression.Call(obj, selectMany, paras!);
        }

        return node;

    }

    protected override Expression VisitExtension(Expression node)
    {
        if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(IQueryable<>))
        {
            var type = node.Type.GenericTypeArguments[0];
            var whereExpr = GetWhereException(type);
            if (whereExpr == null)
            {
                return base.VisitExtension(node);
            }

            var whereMethod = WhereMethod.MakeGenericMethod(type);
            return Expression.Call(null, whereMethod, node, whereExpr);
        }

        return base.VisitExtension(node);
    }

    public Expression TranslateSelect(UnaryExpression expression)
    {
        var lambda = (LambdaExpression)expression.Operand;
        _filterMapping = QueryFilterHelper.GetDynamicMappings(typeof(T), lambda.Parameters);
        var parameters = lambda.Parameters.Select(p => (ParameterExpression)Visit(p)).ToList()!;
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
            var parameter = parameterList[mappingItem.ParamIndex];
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




    public Expression? GetWhereException(Type entityType)
    {
        var queryFilters = QueryFilterHelper.GetQueryFilterItems(_queryDto);
        var paramExpr = Expression.Parameter(entityType, "p");
        var filters = new List<Expression>();
        foreach (var queryFilter in queryFilters)
        {
            var val = queryFilter.Value;
            if (val == null)
                continue;
          
            if (_filterMapping.Mappings.TryGetValue(queryFilter.PropertyName, out var mappingItem))
            {
                if (mappingItem.EntityType != entityType)
                {
                    continue;
                }
                if (mappingItem.EntityProperty.PropertyType != val.GetType())
                {
                    val = Convert.ChangeType(val, mappingItem.EntityProperty.PropertyType);
                }

                var memberExpr = Expression.Property(paramExpr, mappingItem.EntityProperty);
                var filterExpr = Expression.Equal(memberExpr, Expression.Constant(val));
                filters.Add(filterExpr);
            }
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
