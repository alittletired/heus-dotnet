using Heus.AspNetCore.TestBase;

namespace Heus.AspNetCore.Tests;

public class AspNetIntegratedTest:WebIntegratedTestBase<AspNetCoreTestModule,Program>
{
    public AspNetIntegratedTest(WebApplicationFactory<AspNetCoreTestModule, Program> factory) : base(factory)
    {
    }
}