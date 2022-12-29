using System.Data.Common;
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Heus.Data.Internal;
internal class DefaultDbConnectionManager : IDbConnectionManager, IScopedDependency
{
    private readonly IConnectionInfoResolver _connectionInfoResolver;
    private readonly DataOptions _options;
    private readonly ILogger<DefaultDbConnectionManager> _logger;

    private readonly Dictionary<string, DbConnectionWrapper> _connections = new();
    public DefaultDbConnectionManager(IConnectionInfoResolver connectionInfoResolver
        , IOptions<DataOptions> options,
     ILogger<DefaultDbConnectionManager> logger)
    {
        _connectionInfoResolver = connectionInfoResolver;
        _options = options.Value;
        _logger = logger;
    }

    public void Dispose()
    {

        _connections.Values.ForEach(c =>
        {
            //if (c.State != ConnectionState.Closed)
            //{
            //    _logger.LogInformation($"close connections,{c.ConnectionString}");

            //}
            c.DbConnection.Dispose();
        });
        _connections.Clear();
    }

    public DbConnectionWrapper GetDbConnection<TDbContext>() where TDbContext : DbContext
    {
        var connectionInfo = _connectionInfoResolver.Resolve(null);
        var connectionString = connectionInfo.ConnectionString;

        var dbConnection = _connections.GetOrAdd(connectionString, (cs) =>
        {
            var dbContextOptionsProvider = _options.DbConnectionProviders.First(p => p.DbProvider == connectionInfo.DbProvider);
            _logger.LogDebug(" connectionString:{ConnectionString},DbContext:{DbContext},DbProvider:{DbProvider}", 
                connectionString, typeof(TDbContext).Name, connectionInfo.DbProvider);
            var connect = dbContextOptionsProvider.CreateConnection(cs);
            connect.Open();
            return new DbConnectionWrapper{
                DbConnection = connect, DbConnectionProvider = dbContextOptionsProvider
            };
        });
        return dbConnection;
    }
}
