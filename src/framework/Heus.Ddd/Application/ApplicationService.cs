using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Application;

public abstract class ApplicationService:IApplicationService,IOnInstantiation
{
    protected IServiceProvider _serviceProvider=null!;
    public void OnInstantiation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected T GetRequiredService<T>() where T:class {
        return _serviceProvider.GetRequiredService<T>();
        }
    protected ICurrentUser CurrentUser => GetRequiredService<ICurrentUser>();

   
}