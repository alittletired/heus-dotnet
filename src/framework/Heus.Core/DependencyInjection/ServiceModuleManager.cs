using Autofac;
using Autofac.Extensions.DependencyInjection;
using Heus.Core.DependencyInjection.Internal;
using Heus.Core.DependencyInjection.Autofac;

namespace Heus.Core.DependencyInjection;

public class ServiceModuleManager 
{
    public Type StartupModuleType { get; }
    public List<ServiceModuleDescriptor> Modules { get; }
    public static List<Type> AdditionalModules { get; } = new();
    public ServiceModuleManager(Type startupModuleType)
    {

        StartupModuleType = startupModuleType;
        Modules = LoadModules();

    }

    public List<ServiceModuleDescriptor> LoadModules()
    {
        var moduleLoader = new ServiceModuleLoader();
        var modules= moduleLoader.LoadModules(StartupModuleType,AdditionalModules);
        return modules;
    }

    public void ConfigureServices(IHostBuilder hostBuilder)
    {
        
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
        {
            containerBuilder.RegisterServiceMiddlewareSource(new ServiceInjectMethodMiddlewareSource());
        }));

        hostBuilder.ConfigureServices((hostBuilderContext, services) =>
        {
            services.AddHostedService<ModuleHostService>();
            services.AddSingleton(this);
          
            var context = new ServiceConfigurationContext(hostBuilderContext, services);
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
                    var chain = new ServiceRegistrarChain(context.ServiceRegistrars);
                    chain.Next(context.Services, type);
                    serviceTypes.Add(type);
                }

                module.Instance.ConfigureServices(context);
            }
         
            foreach (var postConfigureServices in Modules)
            {
                postConfigureServices.Instance.PostConfigureServices(context);

            }

        });


    }

    public void Configure(IApplicationBuilder  applicationBuilder)
    {
        foreach (var module in Modules)
        {
            module.Instance.Configure(applicationBuilder);
        }
        
    }
}

