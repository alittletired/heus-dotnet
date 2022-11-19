using Autofac;
using Autofac.Extensions.DependencyInjection;
using Heus.Core.DependencyInjection.Autofac;
using Heus.Core.DependencyInjection.Internal;
using Microsoft.Extensions.Options;

namespace Heus.Core.DependencyInjection;

public class DefaultModuleManager : IModuleManager
{
    public Type StartupModuleType { get; }
    public List<ServiceModuleDescriptor> Modules { get; }

    public DefaultModuleManager(IServiceCollection services, Type startupModuleType)
    {
        StartupModuleType = startupModuleType;
        Modules = LoadModules();
    }

    private List<ServiceModuleDescriptor> LoadModules()
    {
        var moduleLoader = new ServiceModuleLoader();
        var modules = moduleLoader.LoadModules(StartupModuleType, ModuleCreateOptions.AdditionalModules);
        return modules;
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IModuleManager>(this);
        services.AddHostedService<ModuleHostService>();
        var serviceRegistrar = new DefaultServiceRegistrar();
        services.AddSingleton<IServiceRegistrar>(serviceRegistrar);
        var context = new ServiceConfigurationContext(services, configuration);
        var serviceTypes = new HashSet<Type>();
        foreach (var preConfigureServices in Modules)
        {
            preConfigureServices.Instance.PreConfigureServices(context);

        }

        //ConfigureServices

        foreach (var module in Modules)
        {
            var assembly = module.Type.Assembly;

            var types = assembly.GetTypes()
                .Where(type => !serviceTypes.Contains(type) &&
                               type.IsClass &&
                               !type.IsAbstract &&
                               !type.IsGenericType).ToList();
            foreach (var type in types)
            {
                serviceRegistrar.Registrar(context.Services, type);
                serviceTypes.Add(type);
            }

            module.Instance.ConfigureServices(context);
        }

        foreach (var postConfigureServices in Modules)
        {
            postConfigureServices.Instance.PostConfigureServices(context);

        }
    }

    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
        {
            containerBuilder.RegisterServiceMiddlewareSource(new ServiceInjectMethodMiddlewareSource());
        }));
        ConfigureServices(builder.Services, builder.Configuration);
    }

    public void Configure(IApplicationBuilder applicationBuilder)
    {
        foreach (var module in Modules)
        {
            module.Instance.Configure(applicationBuilder);
        }

    }
}

