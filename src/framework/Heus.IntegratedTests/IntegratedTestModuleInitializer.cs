using Autofac.Core;
using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
using Heus.Core.Http;
using Heus.Data.EfCore;
using Heus.Data.Mysql;
using Heus.Data.Sqlite;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;

namespace Heus.IntegratedTests;
[DependsOn(typeof(AspNetModuleInitializer)
     , typeof(MysqlModuleInitializer)
    , typeof(SqliteModuleInitializer))]
public class IntegratedTestModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        services.AddSingleton<ITestServerAccessor, TestServerAccessor>();
        services.Remove(services.First(s => s.ServiceType == typeof(IProxyHttpClientFactory)));
        services.AddSingleton<IProxyHttpClientFactory, TestProxyHttpClientFactory>();
        services.AddSingleton<IRemoteServiceProxyContributor, TestRemoteServiceProxyContributor>();

        services.Configure<DbContextConfigurationOptions>(options =>
        {
            //options.DefaultDbProvider = Core.Data.Options.DbProvider.Sqlite;
        });
        base.ConfigureServices(context);
    }
    public override Task Configure(ApplicationConfigurationContext context)
    {
        context.ServiceProvider.GetRequiredService<ITestServerAccessor>().Server =(TestServer) context.ServiceProvider.GetRequiredService<IServer>();
        return Task.CompletedTask;
    }
}