namespace Heus.Core.Data.Options;

public class DbConnectionInfo
{
    
    public DbConnectionInfo(string connectionString, DbProvider provider)
    {
        ConnectionString = connectionString;
        Provider = provider;
    }

    public string ConnectionString { get; set; }

    public DbProvider Provider { get; set; }
}
