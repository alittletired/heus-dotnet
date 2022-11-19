using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.Internal;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.Data;
[DependsOn(typeof(CoreModuleInitializer))]
public class DataModuleInitializer : ModuleInitializerBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OnDbContextScan(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
        context.Services.AddScoped(typeof(IDbContextFactory<>), typeof(DefaultDbContextFactory<>));
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        
    }

    private static void OnDbContextScan(IServiceCollection services)
    {
        var entityDbContextMappings = new Dictionary<Type, Type>();
        services.OnScan(type =>
        {
            if (!typeof(IDbContext).IsAssignableFrom(type))
            {
                return;
            }

            var types = DbContextHelper.GetEntityTypes(type);
            foreach (var entityType in types)
            {
                entityDbContextMappings.Add(entityType, type);
            }
        });

        services.Configure<DataOptions>(options =>
        {
            options.EntityDbContextMappings.AddRange(entityDbContextMappings);
        });

    }
}