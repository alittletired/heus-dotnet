
using Heus.Core.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Heus.Core.Caching;

internal class DefaultDistributedCache<TCacheItem> : DefaultDistributedCache<TCacheItem, string>, IDistributedCache<TCacheItem> where TCacheItem : class
{
    public DefaultDistributedCache(IDistributedCache cache, IOptions<DistributedCacheEntryOptions> options) : base(cache, options)
    {
    }
}
internal class DefaultDistributedCache<TCacheItem, TCacheKey> : IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
    protected IDistributedCache Cache { get; }
    protected SemaphoreSlim SyncSemaphore { get; }
    protected IOptions<DistributedCacheEntryOptions> _options;
    public DefaultDistributedCache(IDistributedCache cache, IOptions<DistributedCacheEntryOptions> options)
    {
        SyncSemaphore = new SemaphoreSlim(1, 1);
        Cache = cache;
        _options= options;
    }

    public async Task<TCacheItem?> GetAsync(TCacheKey key)
    {
        var json = await Cache.GetStringAsync(NormalizeKey(key));
        if(json.IsNullOrEmpty())return null;
        var item= JsonUtils.Deserialize<TCacheItem>(json);
        return item;
    }

    public async Task<TCacheItem> GetOrAddAsync(TCacheKey key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null)
    {
        var value = await GetAsync(key);

        if (value != null)
        {
            return value;
        }
        using (await SyncSemaphore.LockAsync())
        {
            value = await GetAsync(key);
            if (value != null)
            {
                return value;
            }
            value = await factory();
            await SetAsync(key, value, optionsFactory?.Invoke());
            return value;
        }
    }

    public Task RemoveAsync(TCacheKey key)
    {
        return Cache.RemoveAsync(NormalizeKey(key));
    }
    protected virtual string NormalizeKey(TCacheKey key)
    {
        return key!.ToString()!;
    }
    public async Task SetAsync(TCacheKey key, TCacheItem value, DistributedCacheEntryOptions? options = null)
    {
        options = options ?? _options.Value;
        var cacheKey= NormalizeKey(key);
        var item = JsonUtils.Serialize(value);
        options = options ?? _options.Value;
        await Cache.SetStringAsync(cacheKey, item, options);
    }
}
