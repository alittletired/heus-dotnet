using Heus.Core.DependencyInjection;
using Heus.Core.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;

namespace Heus.AspNetCore.TestBase;
[Dependency(Lifetime= ServiceLifetime.Singleton,ReplaceServices =true)]
public class TestProxyHttpClientFactory : IProxyHttpClientFactory
{
    private readonly TestServer _server;


    public TestProxyHttpClientFactory(IServer server)
    {
        _server = (TestServer)server;
    }

    public HttpClient CreateClient(string name)
    {
        return _server.CreateClient();
    }
}