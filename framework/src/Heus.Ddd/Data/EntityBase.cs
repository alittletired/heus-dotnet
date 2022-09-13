
namespace Heus.Ddd.Data;

public abstract class EntityBase:IAuditEntity
{
    /// <summary>
    /// 唯一主键，数据库为varchar(24)
    /// </summary>
    public EntityId? Id { get; set; }
    
    /// <summary>
    /// 创建人
    /// </summary>
    public EntityId? CreatedBy { get; set; }
    
    /// <summary>
    /// 更新人
    /// </summary>
    public EntityId? UpdateBy { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedDate { get; set; }=DateTime.Now;
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateDate { get; set; }
}