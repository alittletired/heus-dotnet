using Heus.Auth.Domain;

namespace Heus.Auth.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("app_user")]
public class User : AuditEntity
{
    /// <summary>
    /// 用户账号
    /// </summary>
    [Required]
    public string Account { get; set; } = null!;
    /// <summary>
    /// 用户账号
    /// </summary>
    [Required]
    public string Password { get; set; } = null!;
    /// <summary>
    /// 用户手机
    /// </summary>
    public string Phone { get; set; } = null!;

    [JsonIgnore]
    public string Salt { get; set; } = null!;
    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Normal;
}