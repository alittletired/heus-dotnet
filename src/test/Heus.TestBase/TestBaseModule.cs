using Heus.Core.DependencyInjection;
using Heus.Data.Sqlite;
namespace Heus.TestBase;
[DependsOn(typeof(SqliteModuleInitializer))]
public class TestBaseModule: ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
       
    }
}
