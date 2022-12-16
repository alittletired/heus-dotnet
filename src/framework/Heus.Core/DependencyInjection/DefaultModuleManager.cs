using Autofac;
using Autofac.Extensions.DependencyInjection;
using Heus.Core.DependencyInjection.Autofac;
using Heus.Core.DependencyInjection.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Heus.Core.DependencyInjection;

public class DefaultModuleManager : IModuleManager
{
    public Type StartupModuleType { get; }
    public List<ServiceModuleDescriptor> Modules { get; }

    public DefaultModuleManager(Type startupModuleType)
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
        //services.AddHostedService
        var serviceRegistrar = new DefaultServiceRegistrar();
        services.AddSingleton<IServiceRegistrar>(serviceRegistrar);
        var context = new ServiceConfigurationContext
        {
            ServiceRegistrar = serviceRegistrar,
            Configuration = configuration,
            Services = services
        };
      
        foreach (var preConfigureServices in Modules)
        {
            preConfigureServices.Instance.PreConfigureServices(context);

        }

        foreach (var module in Modules)
        {
            var assembly = module.Type.Assembly;
            serviceRegistrar.RegistrarModule(context.Services, assembly);
            module.Instance.ConfigureServices(context);
        }

        foreach (var postConfigureServices in Modules)
        {
            postConfigureServices.Instance.PostConfigureServices(context);

        }
    }

    public void AddAutofac(IHostBuilder host)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
        {
            //containerBuilder.RegisterServiceMiddlewareSource(new ServiceInjectMethodMiddlewareSource());
        }));

    }

    public async Task InitializeModulesAsync(IServiceProvider serviceProvider)
    {
        foreach (var module in Modules)
        {
         await   module.Instance.InitializeAsync(serviceProvider);
        }

    }
}

