using System.Collections.Concurrent;

namespace Heus.Core.DependencyInjection;

public interface ICachedServiceProvider
{
    object GetRequiredService(Type serviceType);
    T GetRequiredService<T>();
}
internal class CachedServiceProvider: ICachedServiceProvider,IScopedDependency
{
    private IServiceProvider ServiceProvider { get; }
    protected ConcurrentDictionary<Type, Lazy<object>> CachedServices { get; } = new();
    public CachedServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    public object GetRequiredService(Type serviceType)
    {
        return CachedServices.GetOrAdd(serviceType, _ => new Lazy<object>(() => ServiceProvider.GetRequiredService(serviceType))).Value;
    }
    public T GetRequiredService<T>()
    {
        return (T)GetRequiredService(typeof(T));
    }
}
