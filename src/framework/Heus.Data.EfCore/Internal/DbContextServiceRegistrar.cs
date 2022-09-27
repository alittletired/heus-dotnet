using System.Reflection;
using Heus.Core;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore.Internal;

internal class DbContextServiceRegistrar : IServiceRegistrar
{
    public Dictionary<Type, Type> EntityDbContexts = new Dictionary<Type, Type>();
    public void Handle(IServiceCollection services, Type dbContextType, ServiceRegistrarChain chain)
    {
        if (!dbContextType.IsAssignableTo<DbContext>())
        {
            chain.Next(services, dbContextType);
            return;
        }

        var addDbContext = GetType().GetRuntimeMethods().First(m => m.Name == nameof(AddDbContext));
        var actualMethod = addDbContext.MakeGenericMethod(dbContextType);
        actualMethod.Invoke(this, new object[] { services });
        var entityTypes = DbContextHelper.GetEntityTypes(dbContextType);
        foreach (var entityType in entityTypes)
        {
            EntityDbContexts.Add(entityType, dbContextType);
        }

    }

    private void AddDbContext<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped(sp => sp.GetRequiredService<DbContextOptionsFactory>().Create<TContext>());
    
        services.AddDbContext<TContext>(options =>
        {

        });
    }
}