using Heus.Ddd.Data;
using Heus.Hosting;
using Heus.Ioc;
using Microsoft.EntityFrameworkCore;
namespace Heus.AspNetCore;
using Heus.AspNetCore.OpenApi;

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
        var builder = WebApplication.CreateBuilder(args);
        // var assembly = Assembly.GetCallingAssembly();
        // var name = assembly.GetName().Name;
        // builder.WebHost.UseSetting(WebHostDefaults.ApplicationKey, name);
        builder.Services.AddApplication(startModuleType, builder.Environment,builder.Configuration);
        var app = builder.Build();
        app.UseApplication();
        app.Run();
    }
 

}
