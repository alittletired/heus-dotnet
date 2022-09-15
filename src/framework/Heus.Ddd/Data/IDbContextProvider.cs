using Microsoft.EntityFrameworkCore;

namespace Heus.Core.Ddd.Data;

public interface IDbContextProvider
{
    DbContext GetDbContext(Type entityType);
}