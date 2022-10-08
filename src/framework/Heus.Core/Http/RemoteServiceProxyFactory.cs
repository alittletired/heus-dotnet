using System.Collections.Concurrent;
using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;

namespace Heus.Core.Http;

public class RemoteServiceProxyFactory : ISingletonDependency
{
    private readonly ConcurrentDictionary<Type, object> _proxies = new();
    private readonly ICurrentUser _current;
    private readonly IProxyHttpClientFactory _httpClientFactory;
    public RemoteServiceProxyFactory(ICurrentUser current, IProxyHttpClientFactory httpClientFactory)
    {
        _current = current;
        _httpClientFactory = httpClientFactory;
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
            
            return proxy;
        });
        return (T)proxy;
    }
}