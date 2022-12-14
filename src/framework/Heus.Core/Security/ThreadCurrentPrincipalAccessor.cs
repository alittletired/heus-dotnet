using System.Security.Claims;
using Heus.Core.DependencyInjection;

namespace Heus.Core.Security;
public class ThreadCurrentPrincipalAccessor : CurrentPrincipalAccessorBase,ISingletonDependency
{
    protected override ClaimsPrincipal? GetClaimsPrincipal()
    {
        return Thread.CurrentPrincipal as ClaimsPrincipal;
    }
}
