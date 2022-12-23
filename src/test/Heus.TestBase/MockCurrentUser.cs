using System.Security.Claims;
using Heus.Core.Security;

namespace Heus.TestBase;
internal class MockCurrentUser : ICurrentUser
{
    public bool IsAuthenticated => Id != default;
    public long? Id { get; set; }
    public string Name { get; set; }
    public ClaimsPrincipal? Principal { get; set; }


    public MockCurrentUser(string name)
    {
        Name = name;
    }
}
