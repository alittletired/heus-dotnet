using Heus.Core.DependencyInjection;
using Microsoft.AspNetCore.TestHost;

namespace Heus.IntegratedTests;

public interface ITestServerAccessor { TestServer Server { get; set; } }
public  class TestServerAccessor : ITestServerAccessor
{
    public  TestServer Server { get; set; } = null!;
}