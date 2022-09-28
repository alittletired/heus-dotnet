using Heus.Core.DependencyInjection;
using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
namespace Heus.Data.EfCore;
public interface IDbContextProvider: IScopedDependency
{
    Task<DbContext> GetDbContextAsync<TEntity>() where TEntity:class,IEntity;
}