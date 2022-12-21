using Heus.AspNetCore.TestBase;
using Heus.Core.DependencyInjection;

namespace Heus.AspNetCore.Tests;
[DependsOn(typeof(AspNetCoreTestBaseModule))]
[DependsOn(typeof(AspNetModuleInitializer))]
public class AspNetCoreTestModule  : ModuleInitializerBase
{
}
