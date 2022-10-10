
namespace Heus.Core.Security;
public static class CurrentUserExtensions
{
    public static bool IsAuthenticated(this ICurrentUser currentUser)
    {
        return currentUser.Id.HasValue ;
    }
}

