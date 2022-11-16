using System.Reflection;
using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.EfCore.Internal;

using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data;

[DependsOn(typeof( CoreModuleInitializer))]
public class DataModuleInitializer : ModuleInitializerBase
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OnDbContextRegistered(context.Services);
    }

    private static readonly MethodInfo AddDbContextOptionsMethod = typeof(DataModuleInitializer)
        .GetTypeInfo().DeclaredMethods.First(m => m.Name == nameof(AddDbContextOptions));
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
       
    }
    private static void AddDbContextOptions<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped(sp => sp.GetRequiredService<IDbContextOptionsFactory>().Create<TContext>());
          
    }
    private static void OnDbContextRegistered(IServiceCollection services)
    {
        
        var entityDbContextMappings = new Dictionary<Type, Type>();

        services.OnRegistered(type =>
        {
            if (typeof(DbContext).IsAssignableFrom(type))
            {
                var types = DbContextHelper.GetEntityTypes(type);
                foreach (var entityType in types)
                {
                    entityDbContextMappings.Add(entityType, type);
                }
                var actualMethod = AddDbContextOptionsMethod.MakeGenericMethod(type);
                actualMethod.Invoke(null, new object[] { services });
            }
        });

        services.Configure<DataOptions>(options =>
        {
            options.EntityDbContextMappings.AddRange(entityDbContextMappings);
        });
       
    }
}