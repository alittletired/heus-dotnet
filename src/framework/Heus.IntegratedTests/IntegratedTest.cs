using Heus.Core.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Heus.IntegratedTests;

public class IntegratedTest<TStartup>
 where TStartup : class
{
    private WebApplicationFactory<TStartup> _factory =
        new WebApplicationFactory<TStartup>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services => {
                services.AddSingleton<ITestServerAccessor, TestServerAccessor>();
                services.Remove(services.First(s => s.ServiceType == typeof(IProxyHttpClientFactory)));
                services.AddSingleton<IProxyHttpClientFactory, TestProxyHttpClientFactory>();
                services.AddSingleton<IRemoteServiceProxyContributor, TestRemoteServiceProxyContributor>();


            });
        });
    public IntegratedTest()
    {
        Services.GetRequiredService<ITestServerAccessor>().Server = _factory.Server;

    }

    public IServiceProvider Services => _factory.Services;
   
    public T GetServiceProxy<T>(string remoteServiceName) where T : IRemoteService
    {
        return Services.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<T>(remoteServiceName);
    }
}

