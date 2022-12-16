using System.Collections.Concurrent;


namespace Heus.Core.DependencyInjection.Autofac;
internal static class PropertiesAutowiredHelper
{
   private static  ConcurrentDictionary<Type, AutowiredPropertySelector> _selectors = new();
    public static AutowiredPropertySelector GetSelector(Type type)
    {
        return _selectors.GetOrAdd(type, new AutowiredPropertySelector(type));
    }
}
