using Heus.Ddd.Entities;

namespace Heus.Ddd.Internal;
internal interface IDbContextProvider
{
    DbContext CreateDbContext<TEntity>()where TEntity:IEntity;
}