using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;
using Heus.Ddd;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.Mysql;

[DependsOn(typeof(EfCoreModuleInitializer))]
public class MysqlModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DbContextConfigurationOptions>(options =>
        {
            options.DbContextOptionsProviders.Add(new MySqlDbContextOptionsProvider());
        });
    }
}