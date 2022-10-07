using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
using Heus.IntegratedTests;

namespace Heus.Auth.IntegratedTests;

[DependsOn(typeof(AuthServiceModule) 
    ,typeof(IntegratedTestServiceModule))   ]
public class AuthTestServiceModule:ServiceModuleBase
{
 
}