

using Heus.Core.Modularity;

namespace Heus.AspNetCore
{
    public static class ConfigureContextExtensions
    {
        public static IApplicationBuilder GetApplicationBuilder(this ApplicationConfigurationContext context)
        {
            
         
            return (IApplicationBuilder)context.Host;
        }
      
    }
}
