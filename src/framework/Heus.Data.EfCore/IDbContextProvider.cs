using Heus.Core.DependencyInjection;
using Heus.Ddd.Data;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;
namespace Heus.Data.EfCore;

public interface IDbContextProvider : IScopedDependency
{
    DbContext GetDbContext<TEntity>() where TEntity : class, IEntity;
}