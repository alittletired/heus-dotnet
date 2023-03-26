using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Postgres;
using Heus.Data.SqlServer;
using Heus.Mall;

namespace Heus.Enroll.Web;
[ModuleDependsOn<AspNetModuleInitializer>]
[ModuleDependsOn<MallModuleInitializer>]
[ModuleDependsOn<SqlServerModuleInitializer>]
[ModuleDependsOn<PostgresSqlModuleInitializer>]
public class MallWebModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      
    }
}