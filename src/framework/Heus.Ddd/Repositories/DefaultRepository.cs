
using Heus.Core.Uow;
using Heus.Ddd.Entities;

namespace Heus.Ddd.Repositories;

internal class DefaultRepository<TEntity> : RepositoryBase<TEntity>
    where TEntity : class, IEntity
{
    public DefaultRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}