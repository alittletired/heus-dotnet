using Heus.Ddd.Repositories;

namespace Heus.Ddd;

public class DddOptions
{
    public Type DefaultRepositoryType { get;} = typeof(DefaultRepository<>);
    public Dictionary<Type, Type> CustomRepositories { get; }= new();
    public HashSet<Type> EntityTypes { get; } = new();
    public Dictionary<Type, Type> EntityDbContextMappings { get; } = new();
}