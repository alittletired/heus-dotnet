
namespace Heus.Ddd.Entities;

public interface IAuditEntity:IEntity
{
    long? CreatedBy { get; set; }
    long? UpdateBy { get; set; }
    DateTimeOffset CreatedDate { get; set; }
    DateTimeOffset UpdateDate{ get; set; }
}