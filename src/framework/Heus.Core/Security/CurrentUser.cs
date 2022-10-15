using Heus.Core.DependencyInjection;

namespace Heus.Core.Security.Internal;

using System.Security.Claims;
internal class CurrentUser : ICurrentUser,ISingletonDependency
{
    public bool IsAuthenticated => Id!=default ;
    private readonly ICurrentPrincipalAccessor _principalAccessor;
    public virtual Claim? FindClaim(string claimType)
    {
        return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }
    public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }
    public ClaimsPrincipal? Principal => _principalAccessor.Principal;
  

    public long? Id => Principal.FindClaimValue<long>(ClaimTypes.NameIdentifier);

    public string UserName => Principal.FindClaimValue(ClaimTypes.Name)!;
  
}

