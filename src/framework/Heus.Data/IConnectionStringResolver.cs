namespace Heus.Data;

public interface IConnectionStringResolver
{
    string Resolve(string? connectionStringName = null);
}

