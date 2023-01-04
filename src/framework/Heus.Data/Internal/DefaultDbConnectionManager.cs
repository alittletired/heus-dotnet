using System.Data;
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
    private readonly List<DbConnection> _shouldDisposeConnections = new();
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
        _shouldDisposeConnections.ForEach(c =>
        {
            c.Dispose();
        });
        _shouldDisposeConnections.Clear();
        _connections.Clear();
    }

    public DbConnectionWrapper GetDbConnection<TDbContext>() where TDbContext : DbContext
    {
        var connectionInfo = _connectionInfoResolver.Resolve(null);
        var connectionString = connectionInfo.ConnectionString;

        var dbConnection = _connections.GetOrAdd(connectionString, (cs) =>
        {
            var dbContextOptionsProvider =
                _options.DbConnectionProviders.First(p => p.DbProvider == connectionInfo.DbProvider);
            _logger.LogDebug(" connectionString:{ConnectionString},DbContext:{DbContext},DbProvider:{DbProvider}",
                connectionString, typeof(TDbContext).Name, connectionInfo.DbProvider);
            var connect = dbContextOptionsProvider.CreateConnection(cs);
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();  
                _shouldDisposeConnections.Add(connect);
            }
           
            return new DbConnectionWrapper
            {
                DbConnection = connect, DbConnectionProvider = dbContextOptionsProvider
            };
        });
        return dbConnection;
    }
}
