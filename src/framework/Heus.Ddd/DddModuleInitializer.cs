using Heus.Core.DependencyInjection;

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
    
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        _repositoryRegistrar.RegisterDefaultRepositories(context.Services);
    }
}