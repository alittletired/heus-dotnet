using System.ComponentModel.DataAnnotations.Schema;
using Heus.Ddd.Entities;

namespace Heus.Auth.Entities;
[Table("auth_resource")]
[Index(nameof(Path), IsUnique = true)]
[Index(nameof(Code), IsUnique = true)]
public class Resource : AuditEntity, ITreeEntity,ISoftDelete
{
    public string? AppCode { get; set; }
    public string Name { get; init; } = null!;
    public string Path { get; set; } = null!;
    public bool IsDeleted { get; set; }

    public ResourceType Type { get; set; } = ResourceType.Menu;


    public int Sort { get; set; }
    public string TreeCode { get; set; } = null!;
   
    public string Code { get; set; } = null!;
    public long? ParentId { get; set; }



   
}