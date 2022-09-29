

using Heus.Core.DependencyInjection;

namespace Heus.Core.Data;

public interface IConnectionStringResolver: ISingletonDependency
{
    Task<string> ResolveAsync(string? connectionStringName = null);
}

