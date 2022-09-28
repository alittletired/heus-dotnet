
using Heus.Core.DependencyInjection;
using Heus.Ddd.Data.Options;
using Microsoft.Extensions.Options;

namespace Heus.Data;
internal class DefaultConnectionStringResolver: IConnectionStringResolver
{
    private readonly IOptions<DbConnectionOptions> _options;
     public DefaultConnectionStringResolver(IOptions<DbConnectionOptions> options)
    {
        _options = options;
    }
    public Task<string> ResolveAsync(string? connectionStringName = null)
    {
        return Task.FromResult(ResolveInternal(connectionStringName));
    }
    private string ResolveInternal(string? connectionStringName)
    {
        if (connectionStringName == null)
        {
            return _options.Value.ConnectionStrings.Default;
        }

        return _options.Value.ConnectionStrings[connectionStringName];

     
    }
}