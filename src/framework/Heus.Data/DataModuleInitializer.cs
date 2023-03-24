using System.Reflection;
using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.Internal;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.Data;
[ModuleDependsOn<CoreModuleInitializer>]
public class DataModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped(typeof(IDbContextOptionsFactory<>), typeof(DbContextOptionsFactory<>));
        context.Services.AddScoped(typeof(IDbContextFactory<>), typeof(DefaultDbContextFactory<>));
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
    }

 

    

}