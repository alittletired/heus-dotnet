using Heus.AspNetCore.TestBase;
using Heus.Core.DependencyInjection;
using Heus.TestBase;

namespace Heus.AspNetCore.Tests;
[DependsOn(typeof(AspNetCoreTestBaseModule))]
[DependsOn(typeof(AspNetModuleInitializer))]

public class AspNetCoreTestModule  : ModuleInitializerBase
{
}
