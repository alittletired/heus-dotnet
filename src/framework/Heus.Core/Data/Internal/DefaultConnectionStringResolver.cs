
using Heus.Core.DependencyInjection;
using Heus.Core.Data.Options;
using Microsoft.Extensions.Options;

namespace Heus.Core.Data.Internal;
internal class DefaultConnectionStringResolver: IConnectionStringResolver
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