using System.Security.Claims;
using Heus.Core.Utils;

namespace Heus.Core.Security.Internal;

public abstract class CurrentPrincipalAccessorBase:ICurrentPrincipalAccessor
{
    protected abstract ClaimsPrincipal? GetClaimsPrincipal();
    private readonly AsyncLocal<ClaimsPrincipal?> _currentPrincipal = new();
    public ClaimsPrincipal? Principal => _currentPrincipal.Value??GetClaimsPrincipal();
    public IDisposable Change(ClaimsPrincipal principal)
    {
       return AsyncLocalUtils.BeginScope(_currentPrincipal, principal);
    }
}