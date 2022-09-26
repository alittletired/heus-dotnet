

using Heus.Core.DependencyInjection;

namespace Heus.Data;

public interface IConnectionStringResolver: ISingletonDependency
{
    Task<string> ResolveAsync(string? connectionStringName = null);
}

