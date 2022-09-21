
using Heus.Core.Modularity;

namespace Heus.AspNetCore;

/// <summary>
/// web应用
/// </summary>
public static class WebApplicationHelper
{
    public static async Task<int> RunAsync(string[] args, Type startModuleType)
    {
        try
        {


            var builder = WebApplication.CreateBuilder(args);
            var moduleManager = new ServiceModuleManager(startModuleType);
            var context = new ServiceConfigurationContext(builder);
            moduleManager.ConfigureServices(context);
            var app = builder.Build();
            var appContext = new ApplicationConfigurationContext(app);
            moduleManager.ApplicationInitialize(appContext);
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return 1;
        }
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



