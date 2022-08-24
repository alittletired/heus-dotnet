

namespace Heus.Data;

public interface IConnectionStringResolver
{
    Task<string> ResolveAsync(string? connectionStringName = null);
}

