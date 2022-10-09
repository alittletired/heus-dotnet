

using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.DependencyInjection;
using Heus.Data.Mysql;

namespace Heus.Enroll.Web;
[DependsOn(typeof(AspNetServiceModule),typeof(EnrollServiceModule) 
    ,typeof(EnrollServiceModule) ,typeof(MysqlServiceModule))   ]
public class WebServiceModule:ServiceModuleBase
{
    
}