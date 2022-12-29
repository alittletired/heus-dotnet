
using Heus.Core.DependencyInjection;
using Heus.Ddd.TestModule;
using Heus.TestBase;

namespace Heus.Ddd.Tests;
[DependsOn(typeof (TestBaseModule))]
[DependsOn(typeof(DddTestModuleInitializer))]
public class DddTestModule :ModuleInitializerBase
{
    
}
