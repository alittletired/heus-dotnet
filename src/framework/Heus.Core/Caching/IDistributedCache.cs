using Microsoft.Extensions.Caching.Distributed;
namespace Heus.Core.Caching;
public interface IDistributedCache<TCacheItem> : IDistributedCache<TCacheItem, string>
    where TCacheItem : class
{
   
}
public interface IDistributedCache<TCacheItem, in TCacheKey> where TCacheItem : class
{

    Task<TCacheItem?> GetAsync(TCacheKey key);
    Task<TCacheItem> GetOrAddAsync(TCacheKey key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = default);
    Task SetAsync(TCacheKey key, TCacheItem value, DistributedCacheEntryOptions? options = default);
    Task RemoveAsync(TCacheKey key);

}