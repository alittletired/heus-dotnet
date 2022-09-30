using Heus.Core.DependencyInjection;
using Heus.Core.ObjectMapping;
using Heus.Core.Security;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Application;

public abstract class ApplicationService<TEntity> : IApplicationService, IInitialization where TEntity : class,IEntity
{
    protected IServiceProvider ServiceProvider { get; private set; }= null!;
    protected IObjectMapper Mapper => GetRequiredService<IObjectMapper>();
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