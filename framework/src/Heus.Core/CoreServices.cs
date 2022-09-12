using Heus.Core.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Heus.Core.Ioc.Internal;

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

        public void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(this);
            var serviceTypes = new HashSet<Type>();
            var registrar = new DefaultServiceRegistrar();
            var preConfigureServicesList = Modules
                // ReSharper disable once SuspiciousTypeConversion.Global
                .Select(m =>m.Instance).OfType<IPreConfigureServices>();
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
                                   !type.IsGenericType);
                foreach (var type in types)
                {
                    registrar.Handle(context.Services, type);
                    serviceTypes.Add(type);
                }

                module.Instance.ConfigureServices(context);
            }

        }


        public void ApplicationInitialize(ApplicationConfigurationContext context)
        {

            foreach (var module in Modules)
            {
                module.Instance.ConfigureApplication(context);
            }
        }
    }
}

