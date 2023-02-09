using Heus.Core.DependencyInjection;
using Heus.TestBase;
namespace Heus.Auth.IntegratedTests;
[ModuleDependsOn<AuthModuleInitializer>]
[ModuleDependsOn<TestBaseModule>]
public class AuthTestModule : ModuleInitializerBase
{

}