using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Entities;
[Table("auth_role_resource")]
public class RoleResource : AuditEntity
{
    public long ResourceId { get; set; }
    public long RoleId { get; set; }
}