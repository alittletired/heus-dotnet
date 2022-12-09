using Microsoft.Extensions.Caching.Distributed;

namespace Heus.Core.Caching;

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
