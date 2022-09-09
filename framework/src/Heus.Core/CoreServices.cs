using Heus.Core.Ioc;
using Heus.Ioc.Internal;
using Heus.Ioc;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;


namespace Heus.Core
{
    public class CoreServices
    {
        public Type StartupModuleType { get; }
        public IReadOnlyList<ServiceModuleDescriptor> Modules { get; }

        public CoreServices(Type startupModuleType)
        {

            StartupModuleType = startupModuleType;
            Modules = LoadModules();

        }
        public IReadOnlyList<ServiceModuleDescriptor> LoadModules()
        {
            var moduleLoader = new ServiceModuleLoader();
            return moduleLoader.LoadModules(StartupModuleType);
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(this);
            var serviceTypes = new HashSet<Type>();
            var registrar = new DefaultServiceRegistrar();
            //PreConfigureServices
            var preConfigureServicesList = Modules
                .Where(m => m.Instance is IPreConfigureServices)
                .Select(m => (IPreConfigureServices)m.Instance);

            foreach (var preConfigureServices in preConfigureServicesList)
            {
                preConfigureServices.PreConfigureServices(services);

            }
            //ConfigureServices
            var assemblies = new HashSet<Assembly>();
            foreach (var module in Modules)
            {
                var assembly = module.Type.Assembly;

                var types = assembly.GetTypes()
                    .Where(type => !serviceTypes.Contains(type) &&
                                   type.IsClass &&
                                   !type.IsAbstract &&
                                   !type.IsGenericType);
                foreach (var type in types)
                {
                    registrar.Handle(services, type);
                    serviceTypes.Add(type);
                }

                module.Instance.ConfigureServices(services);
            }

        }


        public void ConfigureApplication(IServiceProvider serviceProvider)
        {
            var context = new ConfigureContext(serviceProvider);
            foreach (var module in Modules)
            {
                module.Instance.Configure(context);
            }
        }
    }
}

