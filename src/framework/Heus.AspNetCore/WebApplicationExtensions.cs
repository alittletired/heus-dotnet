
using Heus.Core.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// web应用
/// </summary>
public static class WebApplicationExtensions
{
    public static WebApplicationBuilder UsingModule(this WebApplicationBuilder builder, Type startModuleType)
    {

        var moduleManager = new ServiceModuleManager(startModuleType);
        moduleManager.ConfigureServices(builder.Host);
        builder.WebHost.Configure(app =>
        {
            var moduleManager = app.ApplicationServices.GetRequiredService<ServiceModuleManager>();
            moduleManager.Configure(app);
        });

        return builder;
    }
    public static WebApplicationBuilder UsingModule<TStartModule>(this WebApplicationBuilder builder)
    {
        return builder.UsingModule(typeof(TStartModule));
    }
}

   
    



