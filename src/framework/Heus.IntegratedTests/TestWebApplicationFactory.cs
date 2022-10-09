using Heus.Core.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Heus.IntegratedTests;

public class TestWebApplicationFactory<TStartup>
: WebApplicationFactory<TStartup> where TStartup : class
{
    public TestWebApplicationFactory()
    {
        Services.GetRequiredService<ITestServerAccessor>().Server = Server;

    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services => {
            services.AddSingleton<ITestServerAccessor, TestServerAccessor>();
            services.Remove(services.First(s => s.ServiceType == typeof(IProxyHttpClientFactory)));
            services.AddSingleton<IProxyHttpClientFactory, TestProxyHttpClientFactory>();
            });
        base.ConfigureWebHost(builder);
    }
    public T GetServiceProxy<T>(string remoteServiceName) where T : IRemoteService
    {
        return Services.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<T>(remoteServiceName);
    }
}

