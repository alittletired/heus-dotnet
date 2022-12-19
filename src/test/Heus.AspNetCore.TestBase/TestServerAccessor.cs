using Heus.Core.DependencyInjection;
using Microsoft.AspNetCore.TestHost;

namespace Heus.AspNetCore.TestBase;

public interface ITestServerAccessor
{
    TestServer? Server { get; set; }
}
internal class TestServerAccessor : ITestServerAccessor, ISingletonDependency
{
    public TestServer? Server { get; set; }
}
