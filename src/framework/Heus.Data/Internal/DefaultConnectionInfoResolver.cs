using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Data.Internal;
internal class DefaultConnectionInfoResolver : IConnectionInfoResolver, ISingletonDependency
{
    private readonly IOptions<DbConnectionOptions> _options;
    public DefaultConnectionInfoResolver(IOptions<DbConnectionOptions> options)
    {
        _options = options;
    }
    public DbConnectionInfo Resolve(string? connectionStringName = null)
    {
        //if (connectionStringName == null)
        //{
        //    return _options.Value.ConnectionStrings.Default;
        //}

        //return _options.Value.ConnectionStrings[connectionStringName];
        //todo:链接字符串不支持数据库类型，目前把类型作为key来传递，暂时不支持connectionStringName
        var (provider,connStr)= _options.Value.ConnectionStrings.First();
        return new DbConnectionInfo(provider, connStr);
    }

}