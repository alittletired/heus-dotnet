using Heus.Ddd.Entities;
using Heus.Ddd.Uow;

namespace Heus.Ddd.Repositories;

internal class DefaultRepository<TEntity> : RepositoryBase<TEntity>
    where TEntity : class, IEntity
{
    public DefaultRepository(IUnitOfWorkManager unitOfWorkManager) : base(unitOfWorkManager)
    {
    }
}