namespace Heus.Core.Data;

public interface IConnectionStringResolver
{
    string Resolve(string? connectionStringName = null);
}

