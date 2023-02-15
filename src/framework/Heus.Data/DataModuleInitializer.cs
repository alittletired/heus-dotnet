using System.Reflection;
using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.Internal;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.Data;
[ModuleDependsOn<CoreModuleInitializer>]
public class DataModuleInitializer : ModuleInitializerBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OnDbContextScan(context);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped(typeof(IDbContextOptionsFactory<>), typeof(DbContextOptionsFactory<>));
        context.Services.AddScoped(typeof(IDbContextFactory<>), typeof(DefaultDbContextFactory<>));
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
    }

    private static MethodInfo _registerDbContext = typeof(DataModuleInitializer)
        .GetTypeInfo().DeclaredMethods.First(m => m.Name == nameof(RegisterDbContext));
    private static void RegisterDbContext<TContext>(IServiceCollection services)where TContext:DbContext
    {
        services.AddScoped(sp => sp.GetRequiredService<IDbContextFactory<TContext>>().CreateDbContext());
    }
    private static void OnDbContextScan(ServiceConfigurationContext context)
    {
        context.ServiceRegistrar.TypeScaning += (_, type) =>
        {
            if (!typeof(DbContext).IsAssignableFrom(type))
            {
                return;
            }
            var registerDbContext = _registerDbContext.MakeGenericMethod(type);
            registerDbContext.Invoke(null, new object[] { context.Services });

        };
    }
   
}