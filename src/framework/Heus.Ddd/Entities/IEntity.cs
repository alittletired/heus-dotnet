using Heus.Ddd.Domain;
using System.Collections.Concurrent;

namespace Heus.Ddd.Entities;

public interface IEntity
{
    long Id { get; set; }
    List<IDomainEvent> DomainEvents { get; }
}