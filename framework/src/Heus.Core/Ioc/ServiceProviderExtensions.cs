using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Core.Ioc;

public static class ServiceProviderExtensions
{
    public static IHostEnvironment GetHostEnvironment(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<IHostEnvironment>();
    }
}