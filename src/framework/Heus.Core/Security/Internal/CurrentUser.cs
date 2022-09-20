﻿namespace Heus.Core.Security.Internal;
using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using System.Security.Claims;
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
