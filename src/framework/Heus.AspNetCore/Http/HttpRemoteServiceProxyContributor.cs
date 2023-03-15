using Heus.Core.DependencyInjection;


namespace Heus.AspNetCore.Http;

public class HttpRemoteServiceProxyContributor : IRemoteServiceProxyContributor,ISingletonDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HttpRemoteServiceProxyContributor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task PopulateRequestHeaders(HttpRequestMessage request)
    {
        var httContext = _httpContextAccessor.HttpContext;
        if (httContext == null)
        {
            return Task.CompletedTask;
        }

        foreach (var header in httContext.Request.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, (string?)header.Value);
        }

        return Task.CompletedTask;
    }
}
