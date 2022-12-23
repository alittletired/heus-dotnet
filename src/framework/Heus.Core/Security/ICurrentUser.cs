
using System.Security.Claims;

namespace Heus.Core.Security;
public interface ICurrentUser
{
    long? Id { get; }
    string Name { get; }
  
    bool IsAuthenticated { get; }

    ClaimsPrincipal? Principal{ get; }


}
