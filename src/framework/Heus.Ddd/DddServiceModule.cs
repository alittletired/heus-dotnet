using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Heus.Ddd.JsonConverters;

namespace Heus.Ddd;

[DependsOn(typeof(CoreServiceModule))]
public class DddServiceModule : ServiceModuleBase, IPreConfigureServices,IPostConfigureServices
{
    private readonly RepositoryRegistrar _repositoryRegistrar = new();
    public void PreConfigureServices(ServiceConfigurationContext context)
    {
      context.AddServiceRegistrar(_repositoryRegistrar);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        JsonUtils.DefaultOptions.Converters.Add(new JsonEntityIdStringConverter());
    }

    public void PostConfigureServices(ServiceConfigurationContext context)
    {
        _repositoryRegistrar.RegisterDefaultRepositories(context.Services);
    }
}