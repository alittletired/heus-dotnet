using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Heus.Business.Auth.Domain;

namespace Heus.Business.Auth.Domain;
[Table("auth_resource")]
public class Resource:AuditEntity,ITreeEntity
{
    /// <summary>
    /// 动作点
    /// </summary>
    [Required]
    public string ActionCode { get; set; } = null!;
    [Required]
    public ResourceType Type{ get; set; } = ResourceType.Menu;

    #region ITreeEntity

    public int Sort { get; set; }
    public string TreeCode { get; set; } = null!;
    public string TreePath { get; set; } = null!;
    public EntityId? ParentId { get; set; } 

    #endregion
   
}