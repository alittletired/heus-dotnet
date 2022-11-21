using Heus.Data.Options;

namespace Heus.Data.Internal;

public interface IConnectionInfoResolver
{
    DbConnectionInfo Resolve(string? connectionStringName = null);
}

