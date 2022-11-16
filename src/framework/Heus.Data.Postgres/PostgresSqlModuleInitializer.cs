using Heus.Core.DependencyInjection;
using Heus.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Heus.Data.Postgres;

[DependsOn(typeof(DataModuleInitializer))]
public class PostgresSqlModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        context.Services.Configure<DataOptions>((options) =>
        {
            options.DbConnectionProviders.Add(new PostgresSqlDbConnectionProvider());
        });

    }
}