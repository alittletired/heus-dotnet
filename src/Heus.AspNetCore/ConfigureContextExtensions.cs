using Heus.Ioc;

namespace Heus.AspNetCore;

public static class ConfigureContextExtensions
{
    public static WebApplication GetApplication(this ConfigureContext context)
    {
        return (WebApplication)context.Host;
    }
}