using Heus.Core.DependencyInjection;
using Heus.Data;

namespace Heus.Data.Sqlite;
[DependsOn(typeof(DataModuleInitializer))]
public class SqliteModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }
}