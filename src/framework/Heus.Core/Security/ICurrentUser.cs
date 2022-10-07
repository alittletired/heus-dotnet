using Heus.Core.DependencyInjection;
using System.Security.Claims;
using Heus.Ddd.Entities;

namespace Heus.Core.Security;
public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    EntityId? UserId { get; }
    string? UserName { get; }

    ClaimsPrincipal? Principal { get; }
   
    Claim? FindClaim(string claimType)
    {
        return Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }
}
