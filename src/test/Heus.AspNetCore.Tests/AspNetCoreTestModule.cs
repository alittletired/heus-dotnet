using Heus.AspNetCore.TestBase;
using Heus.Core.DependencyInjection;
using Heus.Ddd.TestModule;
using Heus.TestBase;

namespace Heus.AspNetCore.Tests;
[ModuleDependsOn<AspNetCoreTestBaseModule>]
[ModuleDependsOn<AspNetModuleInitializer>]
[ModuleDependsOn<DddTestModuleInitializer>]

public class AspNetCoreTestModule  : ModuleInitializerBase
{
}
