using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Utils;
using Heus.Ddd.Repositories;
using Heus.Ddd.Repositories.Filtering;
using Heus.Ddd.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd;

[ModuleDependsOn<DataModuleInitializer>]
public class DddModuleInitializer : ModuleInitializerBase
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OnDbContextScan(context);
        OnRepositoryRegistered(context);
        OnModuleInitialized(context);
    }
    private static MethodInfo _registerDbContext = typeof(DddModuleInitializer)
        .GetTypeInfo().DeclaredMethods.First(m => m.Name == nameof(RegisterDbContext));

    private static void RegisterDbContext<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped(sp =>
        {
            var uow = sp.GetRequiredService<IUnitOfWorkManager>().Current;
            ArgumentNullException.ThrowIfNull(uow);
            return (TContext)uow.GetDbContext(typeof(TContext));
        });
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {

        // var options = context.Services.GetPostOption<DddOptions>();
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
        context.ServiceRegistrar.TypeScaning += (_, type) =>
        {
            if (!typeof(DbContext).IsAssignableFrom(type))
            {
                return;
            }
            var registerDbContext = _registerDbContext.MakeGenericMethod(type);
            registerDbContext.Invoke(null, new object[] { context.Services });
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
        context.ServiceRegistrar.ServiceRegistered += (_, type) =>
        {
            var repoType = type.GetTypeInfo().GetInterfaces().FirstOrDefault(s =>
                s.IsGenericType && s.GetGenericTypeDefinition() == typeof(IRepository<>));
            if (repoType == null)
            {
                return;
            }

            var entityType = repoType.GenericTypeArguments[0];
            customRepositories.Add(entityType, type);
        };
        services.Configure<DddOptions>(options =>
        {
            options.CustomRepositories.AddRange(customRepositories);
        });

    }
    private static void OnModuleInitialized(ServiceConfigurationContext context)
    {
        context.ServiceRegistrar.ModuleInitialized += (_, assembly) => {
            context.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(assembly) );
            };
    }
}