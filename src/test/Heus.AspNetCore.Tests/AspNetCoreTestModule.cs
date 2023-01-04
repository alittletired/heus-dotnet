using Heus.AspNetCore.TestBase;
using Heus.Core.DependencyInjection;
using Heus.Ddd.TestModule;
using Heus.TestBase;

namespace Heus.AspNetCore.Tests;
[DependsOn(typeof(AspNetCoreTestBaseModule))]
[DependsOn(typeof(AspNetModuleInitializer))]
[DependsOn(typeof(DddTestModuleInitializer))]

public class AspNetCoreTestModule  : ModuleInitializerBase
{
}
