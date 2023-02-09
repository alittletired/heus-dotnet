using Heus.Core.DependencyInjection;
using Heus.TestBase;

namespace Heus.AspNetCore.TestBase;
[ModuleDependsOn<TestBaseModule>]
[ModuleDependsOn<AspNetModuleInitializer>]
public class AspNetCoreTestBaseModule: ModuleInitializerBase
{
}
