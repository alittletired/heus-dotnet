
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Ioc;
public static class ServiceCollectionCommonExtensions
{
    public static bool IsAdded(this IServiceCollection services, Type type)
    {
        return services.Any(d => d.ServiceType == type);
    }

    public static Type GetImplementationType(this IServiceCollection services, Type type)
    {
        return services.First(d => d.ServiceType == type).ImplementationType!;
    }
    public static T? GetSingletonInstance<T>(this IServiceCollection services) {
        T instance = default!;
        var service = services.FirstOrDefault(d => d.ServiceType == typeof(T));
        if ( service?.ImplementationInstance != null)
        {
            instance = (T)service.ImplementationInstance;
        }
        return instance;

    }
    public static bool TryGetSingletonInstance<T>(this IServiceCollection services, out T? instance)
    {
        instance = GetSingletonInstance<T>(services);
        return instance!=null;
    }

}