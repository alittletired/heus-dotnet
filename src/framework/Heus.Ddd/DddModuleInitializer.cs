using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Heus.Ddd.JsonConverters;

namespace Heus.Ddd;

[DependsOn(typeof(CoreModuleInitializer))]
public class DddModuleInitializer : ModuleInitializerBase
{
    private readonly RepositoryRegistrar _repositoryRegistrar = new();
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
      context.AddServiceRegistrar(_repositoryRegistrar);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        JsonUtils.DefaultOptions.Converters.Add(new JsonEntityIdStringConverter());
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        _repositoryRegistrar.RegisterDefaultRepositories(context.Services);
    }
}