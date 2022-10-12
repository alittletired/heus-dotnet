using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
using Heus.IntegratedTests;

namespace Heus.Auth.IntegratedTests;

[DependsOn(typeof(AuthModuleInitializer) 
    ,typeof(IntegratedTestModuleInitializer))    ]
public class AuthTestModuleInitializer : ModuleInitializerBase
{
 
}