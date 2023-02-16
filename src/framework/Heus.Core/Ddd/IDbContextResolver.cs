namespace Heus.Ddd.Application;

public interface IDbContextResolver
{
    Type Resolve(Type entityType);
}