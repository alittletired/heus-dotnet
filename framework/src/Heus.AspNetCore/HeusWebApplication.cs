
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
        var builder= Host.CreateDefaultBuilder(args);
        var builder = WebApplication.CreateBuilder(args);
        // var assembly = Assembly.GetCallingAssembly();
        // var name = assembly.GetName().Name;
        // builder.WebHost.UseSetting(WebHostDefaults.ApplicationKey, name);
        builder.Services.AddServiceModule(startModuleType);
        var app = builder.Build();
        //app
        app.Run();
    }
}



