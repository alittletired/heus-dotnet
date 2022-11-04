using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Heus.Auth.Domain;
using Heus.Ddd.Entities;

namespace Heus.Auth.Entities;
[Table("auth_resource")]
[Index(nameof(Code), IsUnique = true)]
public class Resource : AuditEntity, ITreeEntity,ISoftDelete
{
    public string? AppCode { get; set; }
    public string Name { get; init; } = null!;
    public bool IsDeleted { get; set; }
    public ResourceType Type { get; set; } = ResourceType.Menu;
    #region ITreeEntity

    public int Sort { get; set; }
    public string TreeCode { get; set; } = null!;
    [Required]
    public string Code { get; set; } = null!;
    public long? ParentId { get; set; }

    #endregion

   
}