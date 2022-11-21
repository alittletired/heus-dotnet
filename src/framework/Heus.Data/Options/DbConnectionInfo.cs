namespace Heus.Data.Options;

public class DbConnectionInfo
{
    
    public DbConnectionInfo(DbProvider provider,string connectionString)
    {
        ConnectionString = connectionString;
        DbProvider = provider;
    }
    public DbConnectionInfo(string provider, string connectionString) : this(Enum.Parse<DbProvider>(provider,true), connectionString) { }
    public string ConnectionString { get; set; }

    public DbProvider DbProvider { get; set; }
}
