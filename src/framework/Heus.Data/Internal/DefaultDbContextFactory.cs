namespace Heus.Data.Internal;

internal class DefaultDbContextFactory<TContext>:IDbContextFactory<TContext> where TContext : DbContext
{
    public TContext CreateDbContext()
    {
        throw new NotImplementedException();
    }
}