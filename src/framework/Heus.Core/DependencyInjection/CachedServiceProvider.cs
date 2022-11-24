using System.Collections.Concurrent;

namespace Heus.Core.DependencyInjection;

public class CachedServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    protected ConcurrentDictionary<Type, Lazy<object>> CachedServices { get; } = new();
    public CachedServiceProvider(IServiceProvider serviceProvider)

    {
        _serviceProvider = serviceProvider;
    }
    public object GetRequiredService(Type serviceType)
    {
        return CachedServices.GetOrAdd(serviceType, _ => new Lazy<object>(() => _serviceProvider.GetRequiredService(serviceType))).Value;
    }
    public T GetRequiredService<T>()
    {
        return (T)GetRequiredService(typeof(T));
    }
}
