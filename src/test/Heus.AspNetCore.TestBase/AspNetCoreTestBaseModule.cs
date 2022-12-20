using Heus.Core.DependencyInjection;
using Heus.TestBase;

namespace Heus.AspNetCore.TestBase;
[DependsOn(typeof(TestBaseModule))]
[DependsOn(typeof(AspNetModuleInitializer))]
public class AspNetCoreTestBaseModule: ModuleInitializerBase
{
}
