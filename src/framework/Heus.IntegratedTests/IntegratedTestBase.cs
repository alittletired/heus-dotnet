using Heus.AspNetCore;
using Heus.Core.Http;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Heus.IntegratedTests;

public  class IntegratedTestHost<TProgram>
    where TProgram:class 
{
    
    protected readonly WebApplicationFactory<TProgram> _application;
    public IServiceProvider Services => _application.Services;
    public T GetServiceProxy<T>(string remoteServiceName) where T : IRemoteService
    {
        return  _application.Services.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<T>(remoteServiceName); 
    }
    public IntegratedTestHost()
    {
        _application = new WebApplicationFactory<TProgram>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<ITestServerAccessor, TestServerAccessor>();
                services.Replace()<ITestServerAccessor, TestServerAccessor>();

            });
        });
        
        _application.Services.GetRequiredService<ITestServerAccessor>().Server = _application.Server;

    }
}