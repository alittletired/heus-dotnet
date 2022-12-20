using Heus.Core.DependencyInjection;
using Heus.TestBase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;

namespace Heus.AspNetCore.TestBase;

public abstract class AspNetCoreIntegratedTestBase<TStartup> : TestBaseWithServiceProvider, IDisposable where TStartup : class
{
    protected TestServer Server { get; }

    protected HttpClient Client { get; }

    private readonly IHost _host;
    protected AspNetCoreIntegratedTestBase()
    {
        var builder = CreateHostBuilder();
        _host = builder.Build();
        _host.Start();
        Server = _host.GetTestServer();
        Client = _host.GetTestClient();
        ServiceProvider = Server.Services;
        ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
    }
    protected virtual IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TStartup>();

            }).ConfigureServices(services =>
            {
                services.AddScoped<IServer, TestServer>();
            });
       
    }

    protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {

    }

    public void Dispose()
    {
        _host.Dispose();
    }
}
