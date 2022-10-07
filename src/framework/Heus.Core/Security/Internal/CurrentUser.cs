using Heus.Core.DependencyInjection;
using Heus.Ddd.Entities;

namespace Heus.Core.Security.Internal;

using System.Security.Claims;
internal class CurrentUser : ICurrentUser,ISingletonDependency
{
    public bool IsAuthenticated => UserId.HasValue ;
    private readonly ICurrentPrincipalAccessor _principalAccessor;

    public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }
    public ClaimsPrincipal? Principal => _principalAccessor.Principal;

    public EntityId? UserId => this.FindClaimValue<EntityId>(ClaimTypes.NameIdentifier);

    public string? UserName => this.FindClaimValue(ClaimTypes.Name);
  
}

