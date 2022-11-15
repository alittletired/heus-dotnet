using Heus.Core.DependencyInjection;
using Heus.Data;


namespace Heus.Data.Postgres;

[DependsOn(typeof(DataModuleInitializer))]
public class PostgresSqlModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}