using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Heus.Auth.Domain;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth.Entities;
[Table("auth_resource")]
[Index(nameof(Code), IsUnique = true)]
public class Resource : AuditEntity, ITreeEntity,ISoftDelete
{

    [Required]
    public ResourceType Type { get; set; } = ResourceType.Menu;
    public string Code { get; set; } = null!;
    public bool IsDeleted { get; set; }
    #region ITreeEntity

    public int Sort { get; set; }
    public string TreeCode { get; set; } = null!;
    public string TreePath { get; set; } = null!;
    public long? ParentId { get; set; }

    #endregion

   
}