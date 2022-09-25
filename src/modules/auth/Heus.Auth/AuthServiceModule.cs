using Heus.Core.DependencyInjection;
using Heus.Ddd;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Auth
{
    [DependsOn(typeof(DddServiceModule))]
    internal class AuthServiceModule: ServiceModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDbContext<AuthDbContext>(option =>
            {
                //option.UseM
            }); 
        }
    }
}
