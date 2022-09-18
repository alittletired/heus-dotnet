using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.DependencyInjection.Internal;
[Service]
internal class LazyServiceProvider : ILazyServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, Lazy<object>> _cache = new();

    public LazyServiceProvider(IServiceProvider serviceProvider)

    {
        _serviceProvider = serviceProvider;
    }

    public T LazyGetRequiredService<T>() where T : notnull
    {
        return (T)_cache.GetOrAdd(typeof(T), new Lazy<object>(() => _serviceProvider.GetRequiredService<T>())).Value;
    }
}