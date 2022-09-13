namespace Heus.Business.Auth.Domain;

public class RoleResource:AuditEntity
{
    public EntityId ResourceId { get; set; }
    public EntityId RoleId { get; set; }
}