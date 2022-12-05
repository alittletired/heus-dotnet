namespace Heus.AspNetCore;
public static class AspNetServiceProviderExtensions
{
    public static IApplicationBuilder GetApplicationBuilder(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<IApplicationBuilderAccessor>().ApplicationBuilder;
    }
}
