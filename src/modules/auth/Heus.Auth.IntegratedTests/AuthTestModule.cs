using Heus.Core.DependencyInjection;
using Heus.TestBase;
namespace Heus.Auth.IntegratedTests;
[DependsOn(typeof(AuthModuleInitializer)
      , typeof(TestBaseModule)
    )]
public class AuthTestModule : ModuleInitializerBase
{

}