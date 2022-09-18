
using Heus.Core.Modularity;

namespace Heus.AspNetCore;

/// <summary>
/// web应用
/// </summary>
public static class WebApplicationExtensions
{
    public static async Task RunAsync(string[] args, Type startModuleType)
    {
        
        var coreServices = new CoreServices(startModuleType);
        var builder = WebApplication.CreateBuilder(args);
        var context = new ServiceConfigurationContext(builder.Services, builder.Configuration, builder.Environment);
        coreServices.ConfigureServices(context);     
        var app=builder.Build();
        var appContext = new ApplicationConfigurationContext(app);
        coreServices.ApplicationInitialize(appContext);
        await app.RunAsync();
    }

   
    // /// <summary>
    // /// 开启web应用
    // /// </summary>
    // /// <param name="args"></param>
    // /// <param name="startModuleType"></param>
    // public static void Run(string[] args, Type startModuleType)
    // {
    //     var builder= Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
    //     {
    //         webBuilder.Configure((context, app) =>
    //         {
    //             var serviceProvider = app.ApplicationServices;
    //             var host= serviceProvider.GetRequiredService<IHost>();
    //             host.UseServiceModule();
    //
    //         });
    //
    //     });
    //     builder.ConfigureServices(services =>
    //     {
    //         services.AddServiceModule(startModuleType);
    //     });
    //
    //     var app = builder.Build();
    //     app.Run();
    // }
}



