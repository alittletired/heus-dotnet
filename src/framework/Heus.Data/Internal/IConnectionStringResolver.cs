namespace Heus.Data.Internal;

public interface IConnectionStringResolver
{
    string Resolve(string? connectionStringName = null);
}

