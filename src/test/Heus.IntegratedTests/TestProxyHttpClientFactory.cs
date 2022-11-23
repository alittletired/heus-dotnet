using Heus.Core.Http;

namespace Heus.IntegratedTests;

public class TestProxyHttpClientFactory:IProxyHttpClientFactory
{
    private readonly ITestServerAccessor _testServerAccessor;


    public TestProxyHttpClientFactory(ITestServerAccessor testServerAccessor)
    {
        _testServerAccessor = testServerAccessor;
    }

    public HttpClient CreateClient(string name)
    {
        return _testServerAccessor.Server.CreateClient();
    }
}