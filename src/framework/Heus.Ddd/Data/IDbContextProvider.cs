using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
namespace Heus.Core.Ddd.Data;
public interface IDbContextProvider
{
    Task< DbContext> GetDbContextAsync<TEntity>() where TEntity:class,IEntity;
}