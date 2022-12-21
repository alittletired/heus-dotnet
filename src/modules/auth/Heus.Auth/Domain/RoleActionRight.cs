using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Domain;

[Table("auth_role_action_right")]
[Index(nameof(RoleId), nameof(ResourceId), IsUnique = true)]
public class RoleActionRight : AuditEntity
{
    public long ResourceId { get; init; }
    public long RoleId { get; init; }
    public int Flag { get; init; }
}