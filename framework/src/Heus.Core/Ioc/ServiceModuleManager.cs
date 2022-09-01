using Heus.Ioc;
using Heus.Ioc.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Heus.Core.Ioc
{
    internal class ServiceModuleManager
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

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(this);
            var context = new ConfigureServicesContext(services);
            var serviceTypes = new HashSet<Type>();
            var registrar = new DefaultServiceRegistrar();
            //PreConfigureServices
            var preConfigureServicesList = Modules.Where(m => m.Instance is IPreConfigureServices)
                .Select(m => (IPreConfigureServices)m.Instance);

            foreach (var preConfigureServices in preConfigureServicesList)
            {
                preConfigureServices.PreConfigureServices(context);

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
                                   !type.IsGenericType
                    );
                foreach (var type in types)
                {
                    registrar.Handle(services, type);
                    serviceTypes.Add(type);
                }

                module.Instance.ConfigureServices(context);
            }

        }


        public void Configure(IHost host)
        {
            var context = new ConfigureContext(host.Services);
            foreach (var module in Modules)
            {
                module.Instance.Configure(context);
            }
        }
    }
}

