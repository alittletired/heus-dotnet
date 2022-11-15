
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;
namespace Heus.Data;

public interface IDbContextProvider
{
    DbContext GetDbContext<TEntity>() where TEntity : class, IEntity;
}