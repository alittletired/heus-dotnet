
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.Internal;

public class DefaultDbContextFactory<TContext> :IDbContextFactory<TContext> where  TContext:DbContext
{

    private readonly Func<DbContextOptions<TContext>, TContext> _factory = CreateActivator();
    private readonly IDbContextOptionsFactory<TContext> _optionsFactory;
    private static Func<DbContextOptions<TContext>, TContext> CreateActivator()
    {
        var constructor = typeof(TContext).GetTypeInfo()
            .DeclaredConstructors.Single(c => c.IsPublic && c.GetParameters().Length != 0);
        var optionsParameter = Expression.Parameter(typeof(DbContextOptions<TContext>), "options");
        return Expression.Lambda<Func< DbContextOptions<TContext>, TContext>>(
            Expression.New(constructor, optionsParameter), optionsParameter).Compile();

    }

    public DefaultDbContextFactory(IDbContextOptionsFactory<TContext> optionsFactory)
    {
        _optionsFactory = optionsFactory;
    }

    public TContext CreateDbContext()
    {
        return _factory(_optionsFactory.CreateOptions());
    }
}