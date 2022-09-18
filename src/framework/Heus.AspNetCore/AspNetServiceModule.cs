using Heus.AspNetCore.Conventions;
using Heus.AspNetCore.OpenApi;
using Heus.Core.Modularity;
using Heus.Core.Utils;
using Heus.Ddd;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Heus.AspNetCore;

[DependsOn(typeof(DddServiceModule))]
public class AspNetServiceModule : ServiceModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        services.Configure<JsonOptions>(options => {  options.SerializerOptions.ApplyDefaultSettings(); });
        services.AddControllers(options =>
        {
            options.Conventions.Add(new ServiceApplicationModelConvention());
        });
        services.AddOpenApi(context.Environment);

    }

    public override void ConfigureApplication(ApplicationConfigurationContext context)
    {
        var partManager = context.ServiceProvider.GetRequiredService<ApplicationPartManager>();
       var moduleContainer= context.ServiceProvider.GetRequiredService<IModuleContainer>();
       var moduleAssemblies = moduleContainer.Modules.Select(s => s.Assembly).Distinct();
       foreach (var moduleAssembly in moduleAssemblies)
       {
           if (partManager.ApplicationParts.Any(
                   p => p is AssemblyPart assemblyPart && assemblyPart.Assembly == moduleAssembly))
           {
               return;
           }

           partManager.ApplicationParts.Add(new AssemblyPart(moduleAssembly));
       }

       var app = context.GetApplicationBuilder();
        if (context.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi(context.Environment);
        }

        // app.UseHttpsRedirection();
        app.UseStaticFiles();
        // app.UseAuthorization();
        app.UseRouting();
    }

   



}