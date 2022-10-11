
namespace Heus.Ddd.Entities;

public interface IAuditEntity:IEntity
{
    EntityId? CreatedBy { get; set; }
    EntityId? UpdateBy { get; set; }
    DateTimeOffset CreatedDate { get; set; }
    DateTimeOffset UpdateDate{ get; set; }
}