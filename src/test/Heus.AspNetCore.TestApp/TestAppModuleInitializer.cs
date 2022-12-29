using Heus.Core.DependencyInjection;

namespace Heus.AspNetCore.TestApp;

[DependsOn(typeof(AspNetModuleInitializer)
)]
public class TestAppModuleInitializer : ModuleInitializerBase
{
   
}