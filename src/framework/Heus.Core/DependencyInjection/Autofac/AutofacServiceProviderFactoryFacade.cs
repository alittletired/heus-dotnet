using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Heus.Core.DependencyInjection.Autofac;
public class AutofacServiceProviderFactoryFacade : IServiceProviderFactory<ContainerBuilder>
{
    private static void ConfigurationContainerBuilder(ContainerBuilder builder)
    {
        builder.ComponentRegistryBuilder.Registered += (_, args) =>
        {
            // The PipelineBuilding event fires just before the pipeline is built, and
            // middleware can be added inside it.
            args.ComponentRegistration.PipelineBuilding += (_, pipeline) =>
            {
                pipeline.Use(new AutowiredPropertyMiddleware());
            };
        };
    }
    private readonly AutofacServiceProviderFactory _factory = new AutofacServiceProviderFactory(ConfigurationContainerBuilder);
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        var builder = _factory.CreateBuilder(services);

        return builder;
    }


    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        return _factory.CreateServiceProvider(containerBuilder);
    }
}
