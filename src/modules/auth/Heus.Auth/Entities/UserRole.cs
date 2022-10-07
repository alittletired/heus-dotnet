using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Entities;

[Table("auth_user_role")]
public class UserRole : AuditEntity
{
    public EntityId UserId { get; set; }
    public EntityId RoleId { get; set; }
}