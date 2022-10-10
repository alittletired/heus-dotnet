using Heus.Core.DependencyInjection;
using Heus.Ddd.Entities;

namespace Heus.Core.Security.Internal;

using System.Security.Claims;
internal class CurrentUser : ICurrentUser,ISingletonDependency
{
    public bool IsAuthenticated => Id.HasValue ;
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
    public  string? FindClaimValue( string claimType)
    {
        return FindClaim(claimType)?.Value;
    }

    public  T FindClaimValue<T>( string claimType)
        where T : struct
    {
        var value = FindClaimValue(claimType);
        if (value == null)
        {
            return default;
        }

        return value.ConvertTo<T>();
    }

    public EntityId? Id => FindClaimValue<EntityId>(ClaimTypes.NameIdentifier);

    public string? Name => this.FindClaimValue(ClaimTypes.Name);
  
}

