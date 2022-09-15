using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.Ioc;
using Heus.Data.MySql;

namespace Heus.Web;
[DependsOn(typeof(AspNetServiceModule)
    ,typeof(BusinessServiceModule)
    ,typeof(DataServiceModule))]
public class WebServiceModule:ServiceModuleBase
{
    
}