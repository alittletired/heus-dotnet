using System.Collections.Concurrent;
using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Application;

namespace Heus.AspNetCore.Http;

public class RemoteServiceProxyFactory : ISingletonDependency
{
    private readonly ConcurrentDictionary<string, object> _proxies = new();
    
    private readonly IProxyHttpClientFactory _httpClientFactory;
  
      private readonly IEnumerable<IRemoteServiceProxyContributor> _proxyContributors;

      public RemoteServiceProxyFactory(IProxyHttpClientFactory httpClientFactory,
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
    public T CreateProxy<T>(string? remoteServiceName=null) where T : IApplicationService
    {
        var serviceName=  remoteServiceName ?? typeof(T).Assembly.FullName;
        var key = $"{remoteServiceName}:{typeof(T).FullName}";
        var proxy = _proxies.GetOrAdd(key, _ =>
        {
            var proxy = DispatchProxy.Create<T, RemoteServiceProxy>();
            // ReSharper disable once SuspiciousTypeConversion.Global
            var serviceProxy = proxy as RemoteServiceProxy ?? throw new InvalidCastException("无法创建代理");
            serviceProxy.ProxyFactory = this;
            serviceProxy.ProxyType= typeof(T);
            serviceProxy.RemoteServiceName = serviceName!;
            return proxy;
        });
        return (T)proxy;
    }
}