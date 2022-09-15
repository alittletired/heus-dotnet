using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Enroll.Service.Auth.Domain.Entities;

[Table("app_user_role")]
public class UserRole : AuditEntity
{
    public EntityId UserId { get; set; }
    public EntityId RoleId { get; set; }
}