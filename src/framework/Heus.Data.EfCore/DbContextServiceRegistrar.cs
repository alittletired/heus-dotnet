using System.Reflection;
using Heus.Core;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore;

internal class DbContextServiceRegistrar:IServiceRegistrar
{
    public void Handle(IServiceCollection services, Type type, ServiceRegistrarChain chain)
    {
        if (!type.IsAssignableTo<DbContextBase>())
        {
            chain.Next(services,type);
            return;
        }

        var addDbContext = GetType().GetRuntimeMethods().First(m => m.Name == nameof(AddDbContext));
        var actualMethod = addDbContext.MakeGenericMethod(type);
        actualMethod.Invoke(this, new object[] { services });
    }

    private void AddDbContext<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            
        });
    }
}