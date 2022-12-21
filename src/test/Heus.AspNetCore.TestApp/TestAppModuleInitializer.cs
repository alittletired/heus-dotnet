using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
namespace Heus.Enroll.Web;
[DependsOn(typeof(AspNetModuleInitializer)     
    )]
public class TestAppModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
      
    }
}