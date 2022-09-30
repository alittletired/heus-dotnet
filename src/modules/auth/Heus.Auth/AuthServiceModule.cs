using Heus.Core.DependencyInjection;
using Heus.Ddd;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Auth
{
    [DependsOn(typeof(DddServiceModule))]
    public class AuthServiceModule: ServiceModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
          
        }
    }
}
