using System.Security.Claims;
using Heus.Core.DependencyInjection;
using Heus.Core.Security.Internal;

namespace Heus.AspNetCore.Security;

internal class HttpContextCurrentPrincipalAccessor:CurrentPrincipalAccessorBase,ISingletonDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override ClaimsPrincipal? GetClaimsPrincipal()
    {
      return  _httpContextAccessor.HttpContext?.User;
    }
}