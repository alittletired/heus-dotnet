

using Heus.AspNetCore;
using Heus.Business;
using Heus.Core.DependencyInjection;

namespace Heus.Enroll.Web;
[DependsOn(typeof(AspNetServiceModule)
    ,typeof(EnrollServiceModule))   ]
public class WebServiceModule:ServiceModuleBase
{
    
}