

using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.DependencyInjection;
using Heus.Data.Postgres;


namespace Heus.Enroll.Web;
[DependsOn(typeof(AspNetModuleInitializer)
    , typeof(EnrollModuleInitializer)
    , typeof(PostgresSqlModuleInitializer)
    )]
public class WebModuleInitializer : ModuleInitializerBase
{
    
}