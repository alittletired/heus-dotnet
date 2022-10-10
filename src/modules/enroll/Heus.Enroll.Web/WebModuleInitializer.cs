

using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.DependencyInjection;
using Heus.Data.Mysql;

namespace Heus.Enroll.Web;
[DependsOn(typeof(AspNetModuleInitializer) ,typeof(EnrollModuleInitializer) ,typeof(MysqlModuleInitializer))   ]
public class WebModuleInitializer : ModuleInitializerBase
{
    
}