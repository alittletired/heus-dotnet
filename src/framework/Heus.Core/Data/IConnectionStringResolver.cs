

using Heus.Core.DependencyInjection;

namespace Heus.Core.Data;

public interface IConnectionStringResolver: ISingletonDependency
{
    string Resolve(string? connectionStringName = null);
}

