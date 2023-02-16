using Heus.Ddd.Entities;

namespace Heus.Ddd.Tests;

public class TestUser:AuditEntity,ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTimeOffset? DeletedAt { get; set; }
   
}
public class TestUserAddress:IEntity
{
    public long Id { get; set; }
    public long AddressId { get; set; }
    public long UserId { get; set; }
   
}
public class TestAddress:AuditEntity,ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTimeOffset? DeletedAt { get; set; }
   
}