using System.Data.Common;

namespace Heus.Core.Data.Internal;

internal class DefaultDbConnectionProvider:IDbConnectionProvider
{
    public Task<DbConnection> ResolveAsync(string? connectionStringName = null)
    {
        throw new NotImplementedException();
    }
}