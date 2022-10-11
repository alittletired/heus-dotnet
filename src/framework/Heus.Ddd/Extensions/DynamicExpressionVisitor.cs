using Heus.Core.Data.Options;
using Heus.Ddd.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Ddd.Extensions;

internal class DynamicExpressionVisitor<T>: ExpressionVisitor
{
    private readonly DynamicQuery<T> _data;
    private static readonly MethodInfo _selectManyMethod = typeof(Queryable).GetRuntimeMethods()
        .First(m => m.Name == nameof(Queryable.SelectMany) && m.GetParameters().Length == 3);
  public  DynamicExpressionVisitor(DynamicQuery<T> data)
    {
        _data = data;
    }
    [return: NotNullIfNotNull("node")]
    public Expression? VisitStart(Expression? node)
    {
        if (node == null)
            return null;
        if (node is MethodCallExpression methodCall )
        {
            var obj = Visit(methodCall.Object);
            var paras = methodCall.Arguments.Select(Visit).ToArray();
            paras[2] = TranslateSelect((LambdaExpression)paras[2]!);
            var selectMany = methodCall.Method.GetGenericMethodDefinition().MakeGenericMethod(typeof(T));

            return Expression.Call(obj, selectMany, paras!);
        }
        return node;

    }

    public Expression TranslateSelect(LambdaExpression expression) {
        return expression;
        }
    protected override Expression VisitConstant(ConstantExpression node)
    {
        return base.VisitConstant(node);
    }

}
