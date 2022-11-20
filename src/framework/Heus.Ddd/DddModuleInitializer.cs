using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Utils;
using Heus.Ddd.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Ddd;

[DependsOn(typeof(DataModuleInitializer))]
public class DddModuleInitializer : ModuleInitializerBase
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OnDbContextScan(context.Services);
        OnRepositoryRegistered(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
      
        var options = context.Services.GetPostOption<DddOptions>();
        foreach (var entityType in options.EntityDbContextMappings.Keys)
        {
            var entityRepositoryType = typeof(IRepository<>).MakeGenericType(entityType);
            if (!options.CustomRepositories.TryGetValue(entityType, out var repoType))
            {
                repoType = options.DefaultRepositoryType.MakeGenericType(entityType);
            }
            context.Services.AddScoped(entityRepositoryType, repoType);
        }
    }
    private static void OnDbContextScan(IServiceCollection services)
    {
        var entityDbContextMappings = new Dictionary<Type, Type>();
        services.OnScan(type =>
        {
            if (!typeof(DbContext).IsAssignableFrom(type))
            {
                return;
            }

            var types = DbContextUtils.GetEntityTypes(type);
            foreach (var entityType in types)
            {
                entityDbContextMappings.Add(entityType, type);
            }
        });
        services.Configure<DddOptions>(options =>
        {
            options.EntityDbContextMappings.AddRange(entityDbContextMappings);
        });
    }
    private static void OnRepositoryRegistered(IServiceCollection services)
    {

        var customRepositories = new Dictionary<Type, Type>();
        services.OnRegistered(type =>
        {
            var repoType = type.GetTypeInfo().GetInterfaces().FirstOrDefault(s =>
                s.IsGenericType && s.GetGenericTypeDefinition() == typeof(IRepository<>));
            if (repoType != null)
            {
                var entityType = repoType.GenericTypeArguments[0];
                customRepositories.Add(entityType, type);
            }
        });
        services.Configure<DddOptions>(options =>
        {
            options.CustomRepositories.AddRange(customRepositories);
        });

    }
}