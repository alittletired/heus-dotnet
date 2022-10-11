using System.Text;
using Heus.AspNetCore.ActionFilter;
using Heus.AspNetCore.Authorization;
using Heus.AspNetCore.Conventions;
using Heus.AspNetCore.OpenApi;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Heus.Core.Utils;
using Heus.Ddd;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Heus.AspNetCore;

[DependsOn(typeof(DddModuleInitializer))]
public class AspNetModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = context.Configuration;
     
        services.AddMvc(options =>
        {
            options.Conventions.Add(new ServiceApplicationModelConvention());
            options.Conventions.Add(new ApiExplorerGroupConvention());
            options.Filters.AddService(typeof(UowActionFilter));
            options.Filters.AddService(typeof(ApiResultActionFilter));

        }).AddControllersAsServices().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ApplyDefaultSettings();
        });
        services.AddHttpContextAccessor();
        services.AddOpenApi(context.Environment);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.ConfigurationSection)
                    .Get<JwtOptions>() ?? new JwtOptions();
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience=false,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignKey))
                };
            });
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

    }

    public override Task Configure(ApplicationConfigurationContext context)
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
        app.UseRouting();
        // if (context.Environment.IsDevelopment())
        // {
        //     app.UseMiddleware<DevAuthenticationMiddleware>();
        // }
        // app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
        return  Task.CompletedTask;
    }

   



}