
using Heus.Core.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// web应用
/// </summary>
public static class WebApplicationExtensions
{
    public static async Task RunWithModuleAsync(this WebApplicationBuilder builder, Type startModuleType)
    {
      

        var moduleManager = new DefaultModuleManager(builder.Services, startModuleType);
        moduleManager.ConfigureServices(builder.Host);
        var app = builder.Build();
        moduleManager.Configure(app);
        await app.RunAsync();
    }

    public static Task RunWithModuleAsync<TStartModule>(this WebApplicationBuilder builder)
    {
        return builder.RunWithModuleAsync(typeof(TStartModule));
    }
}

   
    



