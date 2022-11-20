
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
        ConstructorInfo[] array = typeof(TContext).GetTypeInfo()
            .DeclaredConstructors.Where(c => !c.IsStatic && c.IsPublic && c.GetParameters().Length != 0).ToArray();
        var optionsParameter = Expression.Parameter(typeof(DbContextOptions<TContext>), "options");
        return Expression.Lambda<Func< DbContextOptions<TContext>, TContext>>(
            Expression.New(array[0], optionsParameter), optionsParameter).Compile();

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