using Heus.Core.DependencyInjection;
using Microsoft.AspNetCore.TestHost;

namespace Heus.IntegratedTests;


public static class TestServerAccessor 
{
    public static TestServer Server { get; set; } = null!;
}