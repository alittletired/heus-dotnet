
using Heus.Core.DependencyInjection;

namespace Heus.Data;
internal class DefaultConnectionStringResolver: IConnectionStringResolver,ISingletonDependency
{
     
    public Task<string> ResolveAsync(string? connectionStringName = null)
    {
        throw new NotImplementedException();
    }
}