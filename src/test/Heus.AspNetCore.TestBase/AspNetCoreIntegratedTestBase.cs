
using System.Net;
using Heus.Core.DependencyInjection;
using Heus.Core.Http;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Heus.AspNetCore.TestBase;

public abstract class AspNetCoreIntegratedTestBase<TStartup,TTestModule> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ModuleCreateOptions.AdditionalModules.Add(typeof(TTestModule));
        builder.UseEnvironment("Testing");
        base.ConfigureWebHost(builder);
    }
    public HttpClient HttpClient => CreateClient();


    public T GetServiceProxy<T>(string remoteServiceName) where T : IRemoteService
    {
        return Services.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<T>(remoteServiceName);
    }
}
