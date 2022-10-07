using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Entities;
[Table("auth_role")]
public class Role : AuditEntity,ISoftDelete
{
    /// <summary>
    /// 内置角色，不允许删除
    /// </summary>
    public bool IsBuildIn { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    /// 角色名
    /// </summary>
    /// <returns></returns>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 角色说明
    /// </summary>
    public  string? Remarks { get; set; }
    
}