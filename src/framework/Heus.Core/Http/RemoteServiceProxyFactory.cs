using System.Collections.Concurrent;
using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;

namespace Heus.Core.Http;

internal class RemoteServiceProxyFactory : ISingletonDependency
{
    private readonly ConcurrentDictionary<Type, object> _proxies = new();
    private readonly ICurrentUser _current;

    public RemoteServiceProxyFactory(ICurrentUser current)
    {
        _current = current;
    }

    public T CreateProxy<T>() where T : IRemoteService
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