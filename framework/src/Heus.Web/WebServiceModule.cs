using Heus.AspNetCore;
using Heus.Business;
using Heus.Core;
using Heus.Data.MySql;
using Heus.Ioc;

namespace Heus.Web;
[DependsOn(typeof(AspNetServiceModule),typeof(BusinessServiceModule),typeof(DataServiceModule))]
public class WebServiceModule:ServiceModuleBase
{
    
}