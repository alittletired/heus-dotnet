using Heus.Core.DependencyInjection;

namespace Heus.AspNetCore.Http;

internal class DefaultProxyHttpClientFactory: IProxyHttpClientFactory,ISingletonDependency
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DefaultProxyHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public HttpClient CreateClient(string name)
    {
        return _httpClientFactory.CreateClient(name);
    }
}