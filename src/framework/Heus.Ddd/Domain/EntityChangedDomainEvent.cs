

namespace Heus.Ddd.Domain;
public class EntityChangedDomainEvent<TEntity>: IDomainEvent
{
    public required long EntityId { get; init; }
}
