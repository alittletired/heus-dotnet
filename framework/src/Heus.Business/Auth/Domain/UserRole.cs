namespace Heus.Business.Auth.Domain;

public class UserRole:AuditEntity
{
    public EntityId UserId { get; set; }
    public EntityId RoleId { get; set; }
}