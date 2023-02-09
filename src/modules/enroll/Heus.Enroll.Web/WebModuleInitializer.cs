using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Postgres;
using Heus.Data.SqlServer;
namespace Heus.Enroll.Web;
[ModuleDependsOn<AspNetModuleInitializer>]
[ModuleDependsOn<EnrollModuleInitializer>]
[ModuleDependsOn<SqlServerModuleInitializer>]
[ModuleDependsOn<PostgresSqlModuleInitializer>]
public class WebModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      
    }
}