using System.Reflection;
using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.EfCore.Internal;
using Heus.Data.EfCore.Repositories;
using Heus.Ddd;
using Heus.Ddd.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data;

public class DataModuleInitializer : ModuleInitializerBase
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDbContextTypes(context.Services);
    }

    private static readonly MethodInfo AddDbContextOptionsMethod = typeof(DataModuleInitializer)
        .GetTypeInfo().DeclaredMethods.First(m => m.Name == nameof(AddDbContextOptions));
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
        context.Services.AddScoped(typeof(IRepositoryProvider<>), typeof(EfCoreRepositoryProvider<>));
    }
    private static void AddDbContextOptions<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped(sp => sp.GetRequiredService<DbContextOptionsFactory>().Create<TContext>());

        // services.AddDbContext<TContext>(options =>
        // {
        //
        // });
    }
    private static void AutoAddDbContextTypes(IServiceCollection services)
    {
        var entityTypes = new List<Type>();
        var entityDbContextMappings = new Dictionary<Type, Type>();

        services.OnRegistered(type =>
        {
            if (typeof(DbContext).IsAssignableFrom(type))
            {
                var types = DbContextHelper.GetEntityTypes(type);
                foreach (var entityType in types)
                {
                    entityDbContextMappings.Add(entityType, type);
                    entityTypes.Add(entityType);
                }

                var actualMethod = AddDbContextOptionsMethod.MakeGenericMethod(type);
                actualMethod.Invoke(null, new object[] { services });
            }
        });

        services.Configure<DbContextConfigurationOptions>(options =>
        {
            options.EntityDbContextMappings.AddRange(entityDbContextMappings);
        });

        services.Configure<RepositoryRegistrationOptions>(options =>
        {
            entityTypes.ForEach(t => options.EntityTypes.Add(t));
        });
    }
}