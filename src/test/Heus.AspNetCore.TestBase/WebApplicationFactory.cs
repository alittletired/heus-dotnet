
using Heus.AspNetCore.Http;
using Heus.Core.DependencyInjection;
using Heus.Core.Extensions;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Heus.AspNetCore.TestBase;

public  class WebApplicationFactory<TTestModule,TStartup> 
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ModuleCreateOptions.AdditionalModules.Add(typeof(TTestModule));
        builder.UseEnvironment(EnvironmentEnvExtensions.Testing);
        base.ConfigureWebHost(builder);
    }
    public HttpClient HttpClient => CreateClient();


    public T GetServiceProxy<T>(string? remoteServiceName=null) where T : IApplicationService
    {
        return Services.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<T>(remoteServiceName);
    }
}
