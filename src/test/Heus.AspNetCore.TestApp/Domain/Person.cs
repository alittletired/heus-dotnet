using Heus.Ddd.Entities;

namespace Heus.AspNetCore.TestApp.Domain;

public class Person:AuditEntity,ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
}
