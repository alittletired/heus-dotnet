
namespace Heus.Ddd.Entities;

public interface IAuditEntity:IEntity
{
    long? CreatedBy { get; set; }
    long? UpdatedBy { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt{ get; set; }
}