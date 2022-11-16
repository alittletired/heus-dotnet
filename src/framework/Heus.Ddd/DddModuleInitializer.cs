using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Ddd.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Ddd;

[DependsOn(typeof(DataModuleInitializer))]
public class DddModuleInitializer : ModuleInitializerBase
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddEntityRepositoryR(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var dataOptions = context.Services.GetPostOption<DataOptions>();
        var options = context.Services.GetPostOption<RepositoryRegistrationOptions>();
        foreach (var entityType in dataOptions.EntityDbContextMappings.Keys)
        {
            var entityRepositoryType = typeof(IRepository<>).MakeGenericType(entityType);
            if (!options.CustomRepositories.TryGetValue(entityType, out var repoType))
            {
                repoType = options.DefaultRepositoryType.MakeGenericType(entityType);
            }
            context.Services.AddScoped(entityRepositoryType, repoType);
        }
    }

    private static void AutoAddEntityRepositoryR(IServiceCollection services)
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
        services.Configure<RepositoryRegistrationOptions>(options =>
        {
            options.CustomRepositories.AddRange(customRepositories);
        });

    }
}