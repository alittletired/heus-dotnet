using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Core.Utils;
public static class ExpressionUtils
{
    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
    {
        Type type = typeof(TSource);

        if (propertyLambda.Body is not MemberExpression member)
        {
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a method, not a property.",
                propertyLambda.ToString()));
        }
        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException(string.Format(
            "Expression '{0}' refers to a field, not a property.",
            propertyLambda.ToString()));
        }

        if (propInfo.ReflectedType == null || (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType)))
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a property that is not from type {1}.",
                propertyLambda.ToString(),
                type));

        return propInfo;
    }
}
