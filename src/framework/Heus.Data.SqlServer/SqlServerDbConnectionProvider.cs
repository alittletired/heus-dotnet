using System.Collections.Concurrent;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Heus.Data.SqlServer;

internal class SqlServerDbConnectionProvider : IDbConnectionProvider
{
  
    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {

     
        dbContextOptions.UseSqlServer(shareConnection,
            options =>
            {
                // options.EnableStringComparisonTranslations();
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
    }

    public DbProvider DbProvider => DbProvider.SqlServer;

    public DbConnection CreateConnection(string connectionString)
    {
        return new SqlConnection(connectionString);
    }

   

}
