using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Ddd.Entities;

public abstract class EntityBase:IEntity
{
    /// <summary>
    /// 唯一主键，数据库为varchar(24)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }
}