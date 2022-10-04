using Heus.Core.DependencyInjection;

namespace Heus.Core.Security;

public interface ITokenProvider:IScopedDependency
{
    string CreateToken(Dictionary<string, string> payload, int expirationMinutes = 30);

}