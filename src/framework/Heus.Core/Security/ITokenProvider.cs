using System.Security.Claims;

namespace Heus.Core.Security;

public interface ITokenProvider 
{
    ClaimsPrincipal CreatePrincipal(ICurrentUser user, TokenType tokenType, bool rememberMe=false);
    string CreateToken(ClaimsPrincipal principal);
}