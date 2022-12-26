
using System.Net;
using Heus.Core.DependencyInjection;
using Heus.Core.Extensions;
using Heus.Core.Http;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Heus.AspNetCore.TestBase;

public interface IWebApplicationFactory {
    IServiceProvider Services { get; }
    
}
public  class WebApplicationFactory<TStartup,TTestModule> : WebApplicationFactory<TStartup>, IWebApplicationFactory where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ModuleCreateOptions.AdditionalModules.Add(typeof(TTestModule));
        builder.UseEnvironment(EnvironmentEnvExtensions.Testing);
        base.ConfigureWebHost(builder);
    }
    public HttpClient HttpClient => CreateClient();


    public T GetServiceProxy<T>(string? remoteServiceName=null) where T : IRemoteService
    {
        return Services.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<T>(remoteServiceName);
    }
}
