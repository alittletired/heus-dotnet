
using Heus.Core.DependencyInjection;
using Heus.Ddd.TestModule;
using Heus.TestBase;

namespace Heus.Ddd.Tests;
[ModuleDependsOn<TestBaseModule>]
[ModuleDependsOn<DddTestModuleInitializer>]
public class DddTestModule :ModuleInitializerBase
{
    
}
