using Heus.Core.DependencyInjection;
using Heus.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.Sqlite;
[DependsOn(typeof(DataModuleInitializer))]
public class SqliteModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DataConfigurationOptions>((options) => {
            options.DbConnectionProviders.Add(new SqliteDbConnectionProvider());
        });
    }
}