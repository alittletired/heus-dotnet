using Heus.Ddd.Domain;

namespace Heus.Ddd.Entities;

public interface IEntity
{
    long Id { get; set; }
    List<IDomainEvent> DomainEvents { get; }
}