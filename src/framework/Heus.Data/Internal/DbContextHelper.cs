using Heus.Core.Utils;
using System.Reflection;


namespace Heus.Data.EfCore.Internal;

internal static class DbContextHelper
{
    public static IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return
            from property in dbContextType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            where ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>))
            select property.PropertyType.GenericTypeArguments[0];
    }
}
