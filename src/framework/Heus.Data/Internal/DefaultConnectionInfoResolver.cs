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
        //todo:�����ַ�����֧�����ݿ����ͣ�Ŀǰ��������Ϊkey�����ݣ���ʱ��֧��connectionStringName
        var (provider,connStr)= _options.Value.ConnectionStrings.First();
        return new DbConnectionInfo(provider, connStr);
    }

}