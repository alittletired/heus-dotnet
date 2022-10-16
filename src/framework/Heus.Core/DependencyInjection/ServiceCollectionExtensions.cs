using Microsoft.Extensions.Options;

namespace Heus.Core.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static T GetSingletonInstance<T>(this IServiceCollection services)
    {
        var service = services.GetSingletonInstanceOrNull<T>();
        if (service == null)
        {
            throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
        }

        return service;
    }
    public static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        return (T?)services.FirstOrDefault(d => d.ServiceType == typeof(T))
            ?.ImplementationInstance;
    }

    public static void OnRegistered(this IServiceCollection services, Action<Type> registrationAction)
    {
        var serviceRegistrar = services.GetSingletonInstance<IServiceRegistrar>();
        serviceRegistrar.OnRegistered(registrationAction);
    }

    public static TOptions GetPostOption<TOptions>(this IServiceCollection services) where TOptions:class,new()
    {
        var options = new TOptions();
       var optionActions= services.Where(s => s.ServiceType == typeof(IConfigureOptions<TOptions>))
            .Select(s => s.ImplementationInstance! as IConfigureOptions<TOptions>).ToList();
       var postOptionActions=services.Where(s => s.ServiceType == typeof(IPostConfigureOptions<TOptions>))
           .Select(s => s.ImplementationInstance! as IPostConfigureOptions<TOptions>).ToList();
       optionActions.ForEach(s=>s?.Configure(options));
       postOptionActions.ForEach(s=>s?.PostConfigure(Options.DefaultName,options));
       return options;
    }
}

