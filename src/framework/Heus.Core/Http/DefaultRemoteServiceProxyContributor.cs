using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;

namespace Heus.Core.Http;
internal class DefaultRemoteServiceProxyContributor : IRemoteServiceProxyContributor, IScopedDependency
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly ITokenProvider _tokenProvider;

    public DefaultRemoteServiceProxyContributor(ICurrentPrincipalAccessor currentPrincipalAccessor, ITokenProvider tokenProvider)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
        _tokenProvider = tokenProvider;
    }

    public Task PopulateRequestHeaders(HttpRequestMessage request)
    {
        if (!request.Headers.Contains("Authorization"))
        {
            var principal = _currentPrincipalAccessor.Principal;
            if (principal != null)
            {
                request.Headers.Add("Authorization", "Bearer " + _tokenProvider.CreateToken(principal));
            }
        }
        return Task.CompletedTask;
    }
}
