using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
using Heus.Core.Http;
using Heus.Core.Uow;
using Heus.Data;
using Heus.Data.Sqlite;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;

namespace Heus.IntegratedTests;
[DependsOn(typeof(AspNetModuleInitializer)
    , typeof(SqliteModuleInitializer))]
public class IntegratedTestModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        services.Remove(services.First(s => s.ServiceType == typeof(IProxyHttpClientFactory)));
        services.AddSingleton<ITestServerAccessor, TestServerAccessor>();

        services.AddSingleton<IProxyHttpClientFactory, TestProxyHttpClientFactory>();
      
        base.ConfigureServices(context);
    }

    public override Task InitializeAsync(IServiceProvider serviceProvider)
    {
        serviceProvider.GetRequiredService<ITestServerAccessor>().Server = (TestServer)serviceProvider.GetRequiredService<IServer>();
        UnitOfWorkManagerAccessor.UnitOfWorkManager= serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        return Task.CompletedTask;
    }
}