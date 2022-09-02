
namespace Heus.AspNetCore;
using Heus.Core.Ioc;

/// <summary>
/// web应用
/// </summary>
public static class HeusWebApplication
{

    /// <summary>
    /// 开启web应用
    /// </summary>
    /// <param name="args"></param>
    /// <param name="startModuleType"></param>
    public static void Run(string[] args, Type startModuleType)
    {
        var builder= Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.Configure((context, app) =>
            {
                var serviceProvider = app.ApplicationServices;
                var host= serviceProvider.GetRequiredService<IHost>();
                host.UseServiceModule();

            });

        });
        builder.ConfigureServices(services =>
        {
            services.AddServiceModule(startModuleType);
        });

        var app = builder.Build();
        app.Run();
    }
}



