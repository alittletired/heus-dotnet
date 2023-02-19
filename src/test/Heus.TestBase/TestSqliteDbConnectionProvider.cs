using System.Data.Common;

using Heus.Data.Sqlite;
using Microsoft.Data.Sqlite;
namespace Heus.TestBase;

internal class TestSqliteDbConnectionProvider : SqliteDbConnectionProvider,IDisposable
{
    //内存数据库在单测时只能使用相同的连接
    private  readonly DbConnection _shareConnection;
    public static  string ShareConnectionString => "Filename=:memory:";
    public TestSqliteDbConnectionProvider()
    {
        // _shareConnection = new SqliteConnection($"DataSource=test{DateTime.Now.Ticks};mode=memory;cache=shared");
        _shareConnection = new SqliteConnection(ShareConnectionString);
        _shareConnection.Open();
        _shareConnection.Disposed += (o, e) =>
        {
            Console.WriteLine("not suppose exec");

        };
    }

    public override DbConnection CreateConnection(string connectionString)
    {
        return _shareConnection;
    }

    public void Dispose()
    {
        _shareConnection.Dispose();
    }

   
}
