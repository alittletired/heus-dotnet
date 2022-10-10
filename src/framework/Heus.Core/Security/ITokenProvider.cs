using System.Security.Claims;
using Heus.Core.DependencyInjection;

namespace Heus.Core.Security;

public interface ITokenProvider 
{
    ClaimsPrincipal CreatePrincipal(ICurrentUser user, TokenType tokenType, bool rememberMe=false);
    string CreateToken(ClaimsPrincipal principal);
}