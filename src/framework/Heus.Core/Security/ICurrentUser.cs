using Heus.Core.DependencyInjection;
using System.Security.Claims;
namespace Heus.Core.Security;
public interface ICurrentUser:ISingletonDependency
{
    bool IsAuthenticated { get; }
    long? UserId { get; }
    string? UserName { get; }

    ClaimsPrincipal? Principal { get; }
    IDisposable SetCurrent(ClaimsPrincipal principal);
    Claim? FindClaim(string claimType)
    {
        return Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }
}
