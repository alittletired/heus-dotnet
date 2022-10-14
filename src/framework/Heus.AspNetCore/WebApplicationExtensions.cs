
using Heus.Core.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// web应用
/// </summary>
public static class WebApplicationExtensions
{
    public static WebApplicationBuilder AddModule(this WebApplicationBuilder builder, Type startModuleType)
    {
        var moduleManager = new DefaultModuleManager(builder.Services, startModuleType);
        moduleManager.ConfigureServices(builder);
        return builder;
    }
    public static WebApplicationBuilder AddModule<TModule>(this WebApplicationBuilder builder)
    {
        return builder.AddModule(typeof(TModule));
    }
    public static WebApplication UseModule(this WebApplication app)
    {
        var moduleManager = app.Services.GetRequiredService<IModuleManager>();
        moduleManager.Configure(app);
        return app;
    }
    public static async Task UseModuleAndRunAsync<TModule>(this WebApplicationBuilder builder)
    {
        await builder.AddModule<TModule>()
       .Build()
       .UseModule()
       .RunAsync(); ;
    }
}

   
    



