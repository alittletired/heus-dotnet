using System.Text;
using Heus.AspNetCore.ActionFilter;
using Heus.AspNetCore.Conventions;
using Heus.AspNetCore.ExceptionHandling;
using Heus.AspNetCore.OpenApi;
using Heus.AspNetCore.Validation;
using Heus.Core.DependencyInjection;
using Heus.Core.Extensions;
using Heus.Core.Security;
using Heus.Core.Utils;
using Heus.Ddd;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Heus.AspNetCore;

[ModuleDependsOn<DddModuleInitializer>]
public class AspNetModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = context.Configuration;
        services.AddCors(o => o.AddPolicy("AnyOrigin", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));
        services.AddMvc(options =>
            {
                options.Conventions.Add(new ServiceApplicationModelConvention());
                options.Conventions.Add(new ApiExplorerGroupConvention());
                options.Filters.AddService(typeof(ApiResultActionFilter));
                options.Filters.AddService(typeof(ValidationActionFilter));
                options.Filters.AddService(typeof(UowActionFilter));

            }).ConfigureApplicationPartManager(partManager =>
            {
                var moduleContainer = services.GetSingleton<IModuleManager>()!;
                var moduleAssemblies = moduleContainer.Modules.Select(s => s.Assembly).Distinct();
                foreach (var moduleAssembly in moduleAssemblies)
                {
                    if (!partManager.ApplicationParts.Any(p => p is AssemblyPart assemblyPart && assemblyPart.Assembly == moduleAssembly))
                    {
                        partManager.ApplicationParts.Add(new AssemblyPart(moduleAssembly));
                    }
                }
            })
            .AddControllersAsServices()
            .AddJsonOptions(options => { options.JsonSerializerOptions.ApplyDefaultSettings(); });
        services.AddHttpContextAccessor();
        services.AddOpenApi();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.ConfigurationSection)
                    .Get<JwtOptions>() ?? new JwtOptions();
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = jwtOptions.Issuer,
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
    public override  Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var partManager = serviceProvider.GetRequiredService<ApplicationPartManager>();
        partManager.FeatureProviders.Add(new ServiceControllerFeatureProvider());
   
        var app= serviceProvider.GetApplicationBuilder();
        var env =serviceProvider.GetRequiredService<IWebHostEnvironment>();
        if (env.IsDevelopment() || env.IsTesting())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
        }

        // app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("AnyOrigin");
        app.UseMiddleware<ExceptionHandlingMiddleware>();
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
        return Task.CompletedTask;
    }
    

   



}