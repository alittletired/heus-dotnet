using Heus.Ioc;

namespace Heus.AspNetCore
{
    public static class ConfigureContextExtensions
    {
        public static IApplicationBuilder GetApplicationBuilder(this ConfigureContext context)
        {
            return context.ServiceProvider.GetRequiredService<IApplicationBuilder>();
        }
    }
}
