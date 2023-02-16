
using Heus.Ddd.Entities;

namespace Heus.Ddd.TestModule.Domain;
public class Address : AuditEntity, ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string City { get; set; } = null!;
    public DateTimeOffset? DeletedAt { get; set; }
}