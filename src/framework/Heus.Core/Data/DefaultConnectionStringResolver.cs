

using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Core.Data;
internal class DefaultConnectionStringResolver: IConnectionStringResolver,ISingletonDependency
{
    private readonly IOptions<DbConnectionOptions> _options;
     public DefaultConnectionStringResolver(IOptions<DbConnectionOptions> options)
    {
        _options = options;
    }
    public string Resolve(string? connectionStringName = null)
    {
        if (connectionStringName == null)
        {
            return _options.Value.ConnectionStrings.Default;
        }

        return _options.Value.ConnectionStrings[connectionStringName];
    }

}