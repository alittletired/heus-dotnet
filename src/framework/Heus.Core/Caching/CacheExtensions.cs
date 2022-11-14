using Heus.Core.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.Extensions.DependencyInjection;

public static  class CacheExtensions
{
    public static void AddCache(this IServiceCollection services) {
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();

        services.AddSingleton(typeof(IDistributedCache<>), typeof(DefaultDistributedCache<>));
        services.AddSingleton(typeof(IDistributedCache<,>), typeof(DefaultDistributedCache<,>));

        services.Configure<DistributedCacheEntryOptions>(cacheOptions =>
        {
            cacheOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
        });
    }
}
