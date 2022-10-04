using Heus.Core.DependencyInjection;
using System.Security.Claims;
using Heus.Ddd.Entities;

namespace Heus.Core.Security;
public interface ICurrentUser:ISingletonDependency
{
    bool IsAuthenticated { get; }
    EntityId? UserId { get; }
    string? UserName { get; }

    ClaimsPrincipal? Principal { get; }
    IDisposable SetCurrent(ClaimsPrincipal principal);
    Claim? FindClaim(string claimType)
    {
        return Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }
}
