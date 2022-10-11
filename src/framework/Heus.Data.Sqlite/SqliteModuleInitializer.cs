using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.Sqlite;
[DependsOn(typeof(EfCoreModuleInitializer))]
public class SqliteModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DbContextConfigurationOptions>(options =>
        {
            options.DbContextOptionsProviders.Add(new SqliteDbContextOptionsProvider());
        });
    }
}