using System.Reflection;
using Heus.Core;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore.Internal;

internal interface IDbContextServiceRegistrar: IServiceRegistrar {
    Dictionary<Type, Type> EntityDbContextMapping { get; }
}
internal class DbContextServiceRegistrar : IDbContextServiceRegistrar
{
    public Dictionary<Type, Type> EntityDbContextMapping { get; } = new ();
    public List<IDbContextOptionsProvider> DbContextOptionsProviders { get; } = new ();
    public void Handle(IServiceCollection services, Type type, ServiceRegistrarChain chain)
    {
        if (type.IsAssignableTo<DbContext>())
        {
            var addDbContext = GetType().GetRuntimeMethods().First(m => m.Name == nameof(AddDbContext));
            var actualMethod = addDbContext.MakeGenericMethod(type);
            actualMethod.Invoke(this, new object[] { services });
            var entityTypes = DbContextHelper.GetEntityTypes(type);
            foreach (var entityType in entityTypes)
            {
                EntityDbContextMapping.Add(entityType, type);
            }
            return;
        }
     
        chain.Next(services, type);
        

    }

    private void AddDbContext<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped(sp => sp.GetRequiredService<DbContextOptionsFactory>().Create<TContext>());
    
        services.AddDbContext<TContext>(options =>
        {

        });
    }
}