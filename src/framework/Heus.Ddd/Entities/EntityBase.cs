using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Heus.Ddd.Domain;

namespace Heus.Ddd.Entities;

public abstract class EntityBase : IEntity
{
    /// <summary>
    /// 唯一主键，数据库为varchar(24)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }
}