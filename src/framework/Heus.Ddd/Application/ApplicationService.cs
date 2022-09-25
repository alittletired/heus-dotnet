using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Heus.Ddd.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Application;

public abstract class ApplicationService<TEntity> : IApplicationService, IOnInstantiation where TEntity : class,IEntity
{
    protected IServiceProvider _serviceProvider = null!;
    protected IRepository<TEntity> Repository => GetRequiredService<IRepository<TEntity>>();

    public void OnInstantiation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected T GetRequiredService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    protected ICurrentUser CurrentUser => GetRequiredService<ICurrentUser>();


}