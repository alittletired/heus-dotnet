using Heus.Core.Utils;
using System.Reflection;
namespace Heus.Data.Utils;

public static class DbContextUtils
{
    public static IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return
            from property in dbContextType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            where ReflectionUtils.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>))
            select property.PropertyType.GenericTypeArguments[0];
    }
}
