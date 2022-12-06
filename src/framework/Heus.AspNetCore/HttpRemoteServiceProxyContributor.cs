using Heus.Core.DependencyInjection;
using Heus.Core.Http;
using Heus.Core.Security;

namespace Heus.AspNetCore;

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
        if (httContext != null)
        {
            foreach (var header in httContext.Request.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, (string?)header.Value);
            }
        }

        return Task.CompletedTask;
    }
}
