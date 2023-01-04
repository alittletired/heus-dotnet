using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Options;
using Heus.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.TestBase;
[DependsOn(typeof(SqliteModuleInitializer))]
public class TestBaseModule: ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DataOptions>((options) =>
        {
           
            options.DbConnectionProviders.RemoveAll(s=>s.DbProvider== DbProvider.Sqlite);
            options.DbConnectionProviders.Add(new TestSqliteDbConnectionProvider());
        });
        context.Services.Configure<DbConnectionOptions>((options) =>
        {
            options.ConnectionStrings[nameof(DbProvider.Sqlite)] = TestSqliteDbConnectionProvider.ShareConnectionString;

        });

    }
}
