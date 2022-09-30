using Heus.AspNetCore.ActionFilter;
using Heus.AspNetCore.Conventions;
using Heus.AspNetCore.OpenApi;
using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Heus.Ddd;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using static System.Net.Mime.MediaTypeNames;

namespace Heus.AspNetCore;

[DependsOn(typeof(DddServiceModule))]
public class AspNetServiceModule : ServiceModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        services.Configure<JsonOptions>(options => {  
            options.SerializerOptions.ApplyDefaultSettings();
            });
        services.AddMvc(options =>
        {
            options.Conventions.Add(new ServiceApplicationModelConvention());
            options.Filters.AddService(typeof(UowActionFilter));

        }).AddControllersAsServices();
        services.AddOpenApi(context.Environment);

    }

    public override void ConfigureApplication(ApplicationConfigurationContext context)
    {
        var partManager = context.ServiceProvider.GetRequiredService<ApplicationPartManager>();
        partManager.FeatureProviders.Add(new ServiceControllerFeatureProvider());

        var moduleContainer= context.ServiceProvider.GetRequiredService<IModuleContainer>();
       var moduleAssemblies = moduleContainer.Modules.Select(s => s.Assembly).Distinct();
       foreach (var moduleAssembly in moduleAssemblies)
       {
           if (partManager.ApplicationParts.Any(
                   p => p is AssemblyPart assemblyPart && assemblyPart.Assembly == moduleAssembly))
           {
               continue;
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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }

   



}