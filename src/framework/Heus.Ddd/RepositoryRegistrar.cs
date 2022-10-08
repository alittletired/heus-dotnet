using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd;

public interface IRepositoryRegistrar : IServiceRegistrar
{
    void AddEntity(Type entityType);
    void AddRepository(Type entityType, Type repositoryType);
}

internal class RepositoryRegistrar : IRepositoryRegistrar
{
    public Dictionary<Type, Type> CustomRepositories = new();
    public HashSet<Type> _entityTypes = new();

    public void AddEntity(Type entityType)
    {
        _entityTypes.Add(entityType);
    }

    public void AddRepository(Type entityType, Type repositoryType)
    {
        //不允许重复注册
        CustomRepositories.Add(entityType, repositoryType);
    }

    public void Handle(IServiceCollection services, Type type, ServiceRegistrarChain chain)
    {
        chain.Next(services, type);
        var repoType = type.GetTypeInfo().GetInterfaces().FirstOrDefault(s => s.IsAssignableTo(typeof(IRepository)));
        if (repoType != null)
        {
            var entityType = repoType.GenericTypeArguments[0];
            AddRepository(entityType, type);
        }

    }

    public static readonly Type DefaultRepositoryType = typeof(DefaultRepository<>);
    public static readonly Type RepositoryType = typeof(IRepository<>);

    public void RegisterDefaultRepositories(IServiceCollection services)
    {
        foreach (var entityType in _entityTypes)
        {
            var entityRepositoryType = RepositoryType.MakeGenericType(entityType);
            if (!CustomRepositories.TryGetValue(entityType, out var repoType))
            {
                repoType = DefaultRepositoryType.MakeGenericType(entityType);
            }

            services.AddScoped(entityRepositoryType, repoType);


        }

    }
}