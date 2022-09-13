using Heus.AspNetCore.Conventions;
using Heus.AspNetCore.OpenApi;
using Heus.Business;
using Heus.Core.Ioc;
using Heus.Core.Json;
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
        services.Configure<JsonOptions>(options => { options.SerializerOptions.ApplyDefaultSettings(); });
        services.AddControllers(options => { options.Conventions.Add(new ServiceApplicationModelConvention()); })
            .ConfigureApplicationPartManager(p =>
            {
                var assemblyPart = new AssemblyPart(typeof(BusinessServiceModule).Assembly);
                p.ApplicationParts.Add(assemblyPart);
                p.FeatureProviders.Add(new ServiceControllerFeatureProvider());
            });
        services.AddOpenApi(context.Environment);

    }

    public override void ConfigureApplication(ApplicationConfigurationContext context)
    {
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