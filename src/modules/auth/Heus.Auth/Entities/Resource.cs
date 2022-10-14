using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Heus.Auth.Domain;
using Heus.Ddd.Entities;

namespace Heus.Auth.Entities;
[Table("auth_resource")]
public class Resource : AuditEntity, ITreeEntity,ISoftDelete
{
    /// <summary>
    /// 动作点
    /// </summary>
    [Required]
    public string ActionCode { get; set; } = null!;
    [Required]
    public ResourceType Type { get; set; } = ResourceType.Menu;
    public bool IsDeleted { get; set; }
    #region ITreeEntity

    public int Sort { get; set; }
    public string TreeCode { get; set; } = null!;
    public string TreePath { get; set; } = null!;
    public long? ParentId { get; set; }

    #endregion

   
}