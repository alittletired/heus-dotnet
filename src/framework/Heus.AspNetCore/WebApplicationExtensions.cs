
using Heus.AspNetCore;
using Heus.Core.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// web应用
/// </summary>
public static class WebApplicationExtensions
{
    public static WebApplicationBuilder AddModule(this WebApplicationBuilder builder, Type startModuleType)
    {
        var moduleManager = new DefaultModuleManager(startModuleType);
        moduleManager.ConfigureServices(builder.Services, builder.Configuration);
        return builder;
    }

    public static WebApplicationBuilder AddModule<TModule>(this WebApplicationBuilder builder)
    {
        return builder.AddModule(typeof(TModule));
    }

    public static async Task<WebApplication> UseModule(this WebApplication app)
    {
        app.Services.GetRequiredService<IApplicationBuilderAccessor>().ApplicationBuilder = app;
        var moduleManager = app.Services.GetRequiredService<IModuleManager>();
        await moduleManager.InitializeModulesAsync(app.Services);
        return app;
    }

    public static async Task UseModuleAndRunAsync<TModule>(this WebApplicationBuilder builder)
    {
        var app = builder.AddModule<TModule>().Build();
        await app.UseModule();
        await app.RunAsync();
    }
}

   
    



