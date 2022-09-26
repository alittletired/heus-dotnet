using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore.Repositories;

public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<DbContext> GetDbContextAsync();

}