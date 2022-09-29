using System.Data.Common;

namespace Heus.Core.Data;

public interface IDbConnectionProvider
{
    Task<DbConnection> ResolveAsync(string? connectionStringName = null);
}