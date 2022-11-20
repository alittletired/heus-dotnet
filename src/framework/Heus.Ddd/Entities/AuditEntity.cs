
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Ddd.Entities;

public abstract class AuditEntity:EntityBase,IAuditEntity
{
    /// <summary>
    /// 创建人
    /// </summary>
    public long? CreatedBy { get; set; }
    /// <summary>
    /// 更新人
    /// </summary>
    public long? UpdateBy { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTimeOffset? CreatedDate { get; set; }=DateTime.Now;
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTimeOffset? UpdateDate { get; set; } = DateTime.Now;
}