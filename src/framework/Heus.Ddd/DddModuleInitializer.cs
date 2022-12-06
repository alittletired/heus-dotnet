using System.Reflection;
using Autofac.Core;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Utils;
using Heus.Ddd.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Ddd;

[DependsOn(typeof(DataModuleInitializer))]
public class DddModuleInitializer : ModuleInitializerBase
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OnDbContextScan(context);
        OnRepositoryRegistered(context);
        OnModuleInitialized(context);
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
    private static void OnDbContextScan(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var entityDbContextMappings = new Dictionary<Type, Type>();
        context.ServiceRegistrar.TypeScaning += (obj, type) =>
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
        };
        services.Configure<DddOptions>(options =>
        {
            options.EntityDbContextMappings.AddRange(entityDbContextMappings);
        });
    }
    private static void OnRepositoryRegistered(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var customRepositories = new Dictionary<Type, Type>();
        context.ServiceRegistrar.ServiceRegistered += (obj, type) =>
        {
            var repoType = type.GetTypeInfo().GetInterfaces().FirstOrDefault(s =>
                s.IsGenericType && s.GetGenericTypeDefinition() == typeof(IRepository<>));
            if (repoType != null)
            {
                var entityType = repoType.GenericTypeArguments[0];
                customRepositories.Add(entityType, type);
            }
        };
        services.Configure<DddOptions>(options =>
        {
            options.CustomRepositories.AddRange(customRepositories);
        });

    }
    private static void OnModuleInitialized(ServiceConfigurationContext context)
    {
        context.ServiceRegistrar.ModuleInitialized += (obj, assembly) => {
            context.Services.AddMediatR(assembly);
            };
    }
}