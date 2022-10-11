using System.Collections.Concurrent;
using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Microsoft.Extensions.Options;

namespace Heus.Core.Http;

public class RemoteServiceProxyFactory : ISingletonDependency
{
    private readonly ConcurrentDictionary<Type, object> _proxies = new();
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly IProxyHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEnumerable<IRemoteServiceProxyContributor> _proxyContributors;
    public RemoteServiceProxyFactory(ICurrentPrincipalAccessor principalAccessor,
        IProxyHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor,
        ITokenProvider tokenProvider, IEnumerable<IRemoteServiceProxyContributor> proxyContributors)
    {
        _currentPrincipalAccessor = principalAccessor;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _tokenProvider = tokenProvider;
        _proxyContributors = proxyContributors;
    }

    public async Task PopulateRequestHeaders(HttpRequestMessage request)
    {
        var httContext = _httpContextAccessor.HttpContext;
        if (httContext != null)
        {
            foreach (var header in httContext.Request.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key,(string?)header.Value );
            }
        }
        if (!request.Headers.Contains("Authorization"))
        {
            var principal = _currentPrincipalAccessor.Principal;
            if (principal != null)
            {
                request.Headers.Add("Authorization","Bearer "+_tokenProvider.CreateToken(principal));
            }
        }

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