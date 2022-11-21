using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Postgres;
using Heus.Data.SqlServer;
namespace Heus.Enroll.Web;
[DependsOn(typeof(AspNetModuleInitializer)
    , typeof(EnrollModuleInitializer)
    , typeof(SqlServerModuleInitializer)
    , typeof(PostgresSqlModuleInitializer)
    )]
public class WebModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      
    }
}