namespace Heus.Core.Internal;

using Heus.Core.Utils;
using System.Security.Claims;
[Service]
internal class CurrentUser : ICurrentUser
{
    private static readonly AsyncLocal<ClaimsPrincipal?> CurrentPrincipal = new();
    public bool IsAuthenticated => Principal != null;

    public ClaimsPrincipal? Principal => CurrentPrincipal.Value;

    public long? UserId => this.FindClaimValue<long>(ClaimTypes.NameIdentifier);

    public string? UserName => this.FindClaimValue(ClaimTypes.Name);
    public IDisposable SetCurrent(ClaimsPrincipal principal)
    {
        return AsyncLocalUtils.BeginScope(CurrentPrincipal, principal);
    }
}

