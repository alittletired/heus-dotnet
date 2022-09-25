using System.Data.Common;

namespace Heus.Ddd.Data;

public interface IDbConnectionProvider
{
    Task<DbConnection> ResolveAsync(string? connectionStringName = null);
}