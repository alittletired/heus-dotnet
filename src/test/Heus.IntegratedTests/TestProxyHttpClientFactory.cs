using Heus.Core.Http;

namespace Heus.IntegratedTests;

public class TestProxyHttpClientFactory:IProxyHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        return TestServerAccessor.Server.CreateClient();
    }
}