
using Heus.Ddd.Entities;

namespace Heus.Ddd.TestModule.Domain;
public class Address : AuditEntity, ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;

}