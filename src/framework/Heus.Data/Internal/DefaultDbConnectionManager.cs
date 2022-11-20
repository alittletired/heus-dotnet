using System.Data.Common;
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Heus.Data.Internal;
internal class DefaultDbConnectionManager : IDbConnectionManager, IScopedDependency
{
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly DataOptions _options;
    private readonly ILogger<DefaultDbConnectionManager> _logger;

    private readonly Dictionary<string, DbConnection> _connections = new();
    public DefaultDbConnectionManager(IConnectionStringResolver connectionStringResolver
        , IOptions<DataOptions> options,
     ILogger<DefaultDbConnectionManager> logger)
    {
        _connectionStringResolver = connectionStringResolver;
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
            c.Dispose();
        });
        _connections.Clear();
    }

    public DbConnection GetDbConnection<TDbContext>() where TDbContext : DbContext
    {
     
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();

        var connectionString = _connectionStringResolver.Resolve(connectionStringName);

        var dbConnection = _connections.GetOrAdd(connectionString, (cs) =>
        {
            var dbContextOptionsProvider = _options.DbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
            _logger.LogDebug(" connectionString:{ConnectionString},DbContext:{DbContext}", connectionString, typeof(TDbContext).Name);
            var connect = dbContextOptionsProvider.CreateConnection(cs);
            //connect.Open();
            return connect;
        });


        return dbConnection;
    }
}
