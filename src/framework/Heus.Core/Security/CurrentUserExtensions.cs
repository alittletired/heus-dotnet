
using System.Security.Claims;

namespace Heus.Core.Security;

public static class CurrentUserExtensions
{
    public static bool IsAuthenticated(this ICurrentUser currentUser)
    {
        return currentUser.Id!=default;
    }

    public static T FindClaimValue<T>(this ClaimsPrincipal? principal, string claimType)
        where T : struct
    {
        var value = principal.FindClaimValue(claimType);
        if (value == null)
        {
            return default;
        }

        return value.ConvertTo<T>();

    }

    public static string? FindClaimValue(this ClaimsPrincipal? principal, string claimType)
    {
        return principal?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

    }
}

