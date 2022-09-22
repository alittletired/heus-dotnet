using Microsoft.EntityFrameworkCore;

namespace Heus.Core.Ddd.Data;

public interface IDbContextProvider
{
    Task< DbContext> GetDbContextAsync(Type entityType);
}