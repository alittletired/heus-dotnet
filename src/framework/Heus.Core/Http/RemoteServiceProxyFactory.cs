using System.Collections.Concurrent;
using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;


namespace Heus.Core.Http;

public class RemoteServiceProxyFactory : ISingletonDependency
{
    private readonly ConcurrentDictionary<Type, object> _proxies = new();
    
    private readonly IProxyHttpClientFactory _httpClientFactory;
  
      private readonly IEnumerable<IRemoteServiceProxyContributor> _proxyContributors;
    public RemoteServiceProxyFactory(        IProxyHttpClientFactory httpClientFactory,
      IEnumerable<IRemoteServiceProxyContributor> proxyContributors)
    {
       
        _httpClientFactory = httpClientFactory;
        _proxyContributors = proxyContributors;
    }

    public async Task PopulateRequestHeaders(HttpRequestMessage request)
    {
        foreach (var contributor in _proxyContributors)
        {
          await  contributor.PopulateRequestHeaders(request);
        }
    }

    public HttpClient GetHttpClient(string remoteServiceName)
    {
        return _httpClientFactory.CreateClient(remoteServiceName);
    }
    public T CreateProxy<T>(string remoteServiceName) where T : IRemoteService
    {
        var proxy = _proxies.GetOrAdd(typeof(T), t =>
        {
            var proxy = DispatchProxy.Create<T, RemoteServiceProxy>();
            var serviceProxy = proxy as RemoteServiceProxy ?? throw new InvalidCastException("无法创建代理");
            serviceProxy.ProxyFactory = this;
            serviceProxy.ProxyType= typeof(T);


            return proxy;
        });
        return (T)proxy;
    }
}