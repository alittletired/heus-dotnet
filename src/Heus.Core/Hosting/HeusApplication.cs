using Heus.Ioc;
using Heus.Ioc.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Hosting;

internal class HeusApplication
{
    private readonly List<ServiceModuleDescriptor> _modules;
    private readonly Type _startModuleType;
    public HeusApplication(Type startModuleType)
    {
        if (!startModuleType.Is<IServiceModule>())
        {
            throw new InvalidCastException($"启动模块类型必须继承{nameof(IServiceModule)}");
        }
        _startModuleType = startModuleType;
        var moduleLoader = new ServiceModuleLoader();
        _modules = moduleLoader.LoadModules(startModuleType);
        
    }
   
    public void ConfigureServices(IServiceCollection services, IHostEnvironment environment,IConfiguration configuration)
    {
        services.AddSingleton(this);
        var context = new ConfigureServicesContext(services,environment,configuration);
        var serviceTypes = new HashSet<Type>();
        var registrar = new DefaultServiceRegistrar();
        //ConfigureServices
        foreach (var module in _modules)
        {
            var types = module.Assembly.GetTypes()
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
            try
            {
                module.Instance.ConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occurred during ConfigureServices phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.",
                    ex);
            }
        }
    }

    public void Configure(IHost host)
    {
     
        var context = new ConfigureContext(host);
        
        foreach (var module in _modules)
        {
            module.Instance.Configure(context);
        }
    }
}