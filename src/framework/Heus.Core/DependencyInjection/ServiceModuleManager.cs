using Autofac;
using Autofac.Extensions.DependencyInjection;
using Heus.Core.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Heus.Core.DependencyInjection.Autofac;

namespace Heus.Core.DependencyInjection;

public class ServiceModuleManager : IModuleContainer
{
    public Type StartupModuleType { get; }
    public IReadOnlyList<ServiceModuleDescriptor> Modules { get; }

    public ServiceModuleManager(Type startupModuleType)
    {

        StartupModuleType = startupModuleType;
        Modules = LoadModules();

    }

    public IReadOnlyList<ServiceModuleDescriptor> LoadModules()
    {
        var moduleLoader = new ServiceModuleLoader();
        return moduleLoader.LoadModules(StartupModuleType);
    }

    public void ConfigureServices(IHostBuilder hostBuilder)
    {
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
        {
            containerBuilder.RegisterServiceMiddlewareSource(new ServiceInjectMethodMiddlewareSource());
        }));

        hostBuilder.ConfigureServices((hostBuilderContext, services) =>
        {
            services.AddSingleton(this);
            services.AddSingleton<IModuleContainer>(this);
            var context = new ServiceConfigurationContext(hostBuilderContext, services);
            var serviceTypes = new HashSet<Type>();
           
            var preConfigureServicesList = Modules
                // ReSharper disable once SuspiciousTypeConversion.Global
                .Select(m => m.Instance).OfType<IPreConfigureServices>();
            foreach (var preConfigureServices in preConfigureServicesList)
            {
                preConfigureServices.PreConfigureServices(context);

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
                    var chain = new ServiceRegistrarChain(context.ServiceRegistrars);
                    chain.Next(context.Services, type);
                    serviceTypes.Add(type);
                }

                module.Instance.ConfigureServices(context);
            }
            var postConfigureServicesList = Modules
                // ReSharper disable once SuspiciousTypeConversion.Global
                .Select(m => m.Instance).OfType<IPostConfigureServices>();
            foreach (var postConfigureServices in postConfigureServicesList)
            {
                postConfigureServices.PostConfigureServices(context);

            }

        });


    }

    public async Task ApplicationInitialize(IHost host)
    {
        var context = new ApplicationConfigurationContext(host);
        foreach (var module in Modules)
        {
           await module.Instance.ConfigureApplication(context);
        }
    }
}

