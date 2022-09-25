using System.Data.Common;
using Heus.Ddd.Data;

namespace Heus.Data;

internal class DefaultDbConnectionProvider:IDbConnectionProvider
{
    public Task<DbConnection> ResolveAsync(string? connectionStringName = null)
    {
        throw new NotImplementedException();
    }
}