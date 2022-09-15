using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Enroll.Service.Auth.Domain.Entities;
[Table("app_role_resource")]
public class RoleResource : AuditEntity
{
    public EntityId ResourceId { get; set; }
    public EntityId RoleId { get; set; }
}