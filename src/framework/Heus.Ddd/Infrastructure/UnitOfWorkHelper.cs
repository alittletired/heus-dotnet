using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Heus.DDD.Infrastructure;

public static class UnitOfWorkHelper
{
    public static bool IsUnitOfWorkType(TypeInfo implementationType)
    {
        //Explicitly defined UnitOfWorkAttribute
        if (HasUnitOfWorkAttribute(implementationType) || AnyMethodHasUnitOfWorkAttribute(implementationType))
        {
            return true;
        }
        return false;
    }
    public static UnitOfWorkAttribute? GetUnitOfWorkAttributeOrNull(MethodInfo? methodInfo)
    {
        if (methodInfo == null)
            return null;
        var attr = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().FirstOrDefault();
        if (attr!= null)
        {
            return attr;
        }

        return methodInfo.DeclaringType?.GetTypeInfo().GetCustomAttributes(true)
            .OfType<UnitOfWorkAttribute>().FirstOrDefault();
    }

    private static bool AnyMethodHasUnitOfWorkAttribute(TypeInfo implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasUnitOfWorkAttribute);
    }

    private static bool HasUnitOfWorkAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
    }
}