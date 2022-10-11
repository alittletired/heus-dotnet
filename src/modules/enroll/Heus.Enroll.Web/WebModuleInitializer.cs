

using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.DependencyInjection;
using Heus.Data.Mysql;
using Heus.Data.Sqlite;

namespace Heus.Enroll.Web;
[DependsOn(typeof(AspNetModuleInitializer)
    , typeof(EnrollModuleInitializer)
    , typeof(MysqlModuleInitializer)
    , typeof(SqliteModuleInitializer))]
public class WebModuleInitializer : ModuleInitializerBase
{

}