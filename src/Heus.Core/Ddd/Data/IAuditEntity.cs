
namespace Heus.Ddd.Data;

public interface IAuditEntity:IEntity
{
    EntityId? CreatedBy { get; set; }
    EntityId? UpdateBy { get; set; }
    DateTime? CreatedDate { get; set; }
    DateTime? UpdateDate{ get; set; }
}