using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Entities;
[Table("app_role_resource")]
public class RoleResource : AuditEntity
{
    public EntityId ResourceId { get; set; }
    public EntityId RoleId { get; set; }
}