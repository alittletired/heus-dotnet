using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Entities;

[Table("auth_user_role")]
public class UserRole : AuditEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}