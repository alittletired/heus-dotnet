using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Heus.Ddd.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Application;

public abstract class ApplicationService<TEntity> : IApplicationService, IInitialization where TEntity : class,IEntity
{
    protected IServiceProvider ServiceProvider { get; private set; }= null!;
    protected IRepository<TEntity> Repository => GetRequiredService<IRepository<TEntity>>();
    protected T GetRequiredService<T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    protected ICurrentUser CurrentUser => GetRequiredService<ICurrentUser>();


    public void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
}