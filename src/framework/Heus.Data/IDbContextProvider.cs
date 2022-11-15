
using Heus.Ddd.Entities;

namespace Heus.Data;

public interface IDbContextProvider
{
    DbContext GetDbContext<TEntity>() where TEntity : class, IEntity;
}