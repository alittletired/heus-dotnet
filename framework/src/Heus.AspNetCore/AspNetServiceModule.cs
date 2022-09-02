using Heus.AspNetCore.Conventions;
using Heus.AspNetCore.OpenApi;
using Heus.Business;
using Heus.Ioc;
using Heus.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Heus.AspNetCore;

[DependsOn(typeof(CoreServiceModule))]
public class AspNetServiceModule : ServiceModuleBase
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {

        context.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.ApplyDefaultSettings();
        });
        context.Services.AddControllers(options =>
        {
           options.Conventions.Add(new ServiceApplicationModelConvention());
        }).ConfigureApplicationPartManager(p =>
        {
            var assemblyPart = new AssemblyPart(typeof(BusinessServiceModule).Assembly);
            p.ApplicationParts.Add(assemblyPart);
            p.FeatureProviders.Add(new ServiceControllerFeatureProvider());
        });
        context.Services.AddOpenApi(context.Environment);
        base.ConfigureServices(context);
    }

public override void Configure(ConfigureContext context)
{
    var env = context.Environment;
    var app = context.GetApplicationBuilder();
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseOpenApi(env);
    }

    // app.UseHttpsRedirection();
    app.UseStaticFiles();
    // app.UseAuthorization();
    app.UseRouting();
}


}