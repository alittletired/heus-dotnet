using System.Security.Claims;

namespace Heus.Core.Security;

public interface ICurrentPrincipalAccessor
{
    ClaimsPrincipal? Principal { get; }

    IDisposable Change(ClaimsPrincipal? principal);
}