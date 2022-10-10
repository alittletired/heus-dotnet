using Heus.Core.DependencyInjection;

namespace Heus.Core.Security;

public interface ITokenProvider : IScopedDependency
{
    AuthToken CreateToken(ICurrentUser user, TokenType tokenType);

}