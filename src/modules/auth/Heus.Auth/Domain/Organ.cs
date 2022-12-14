using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Domain;

[Table("auth_organ")]
public class Organ : AuditEntity, ITreeEntity
{
    /// <summary>
    /// 部门地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 部门电话
    /// </summary>
    public string? Phone { get; set; }

    public string Name { get; set; } = null!;

    #region ITreeEntity

    public int Sort { get; set; }
    public string Code { get; set; } = null!;
    public string TreeCode { get; set; } = null!;
    public long? ParentId { get; set; }

    #endregion

}