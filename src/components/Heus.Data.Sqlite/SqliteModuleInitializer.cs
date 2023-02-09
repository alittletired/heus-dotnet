using Heus.Core.DependencyInjection;
using Heus.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.Sqlite;
[ModuleDependsOn<DataModuleInitializer>]
public class SqliteModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DataOptions>((options) => {
            options.DbConnectionProviders.Add(new SqliteDbConnectionProvider());
        });
    }
}