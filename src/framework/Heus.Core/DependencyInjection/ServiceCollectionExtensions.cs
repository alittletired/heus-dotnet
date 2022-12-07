using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Core.DependencyInjection;
public static class ServiceCollectionExtensions
{
  
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

