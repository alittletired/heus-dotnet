using Heus.Core.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Heus.Core.Tests.Caching;
[TestClass]
public class DefaultDistributedCacheTests
{
   private readonly IDistributedCache<string> _cache;

   public DefaultDistributedCacheTests()
   {
     
      var cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
      var options= Options.Create(new DistributedCacheEntryOptions());
      _cache = new DefaultDistributedCache<string>(cache, options);
      
   }
    [TestMethod]
    [DataRow(nameof(GetAsync_Test),null,"test")]
   public async Task GetAsync_Test(string key,object value,string newValue)
   {
       var res = await _cache.GetAsync(key);
    
       res.ShouldBe(value);
       await _cache.SetAsync(key,newValue);
       var res1 = await _cache.GetAsync(key);
       res1.ShouldBe(newValue);
   }

   [TestMethod]
   [DataRow("key1", "1")]
   [DataRow("key2", "2")]
   public async Task GetOrAddAsync_Test(string key, string value)
   {
     var res=await  _cache.GetOrAddAsync(key, ()=>Task.FromResult(value));
     res.ShouldBe(value);
     await  _cache.GetOrAddAsync(key, ()=>Task.FromResult(value));
   }

    [TestMethod]
    [DataRow("key3", "3")]
   [DataRow("key4", "4")]
   public async Task RemoveAsync_Test(string key, string value)
   {
        var res=  await  _cache.GetOrAddAsync(key,()=> Task.FromResult(value));
        res.ShouldBe(value);
        await _cache.RemoveAsync(key);
        var res1 = await _cache.GetAsync(key);
        res1.ShouldBeNull();

    }
}