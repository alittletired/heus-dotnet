using Heus.Auth.Domain;
using Heus.Core.Utils;

namespace Heus.Auth.Entities;

using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("auth_user")]
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
 [JsonIgnore]
    public string Password { get;private set; } = null!;
    /// <summary>
    /// 用户手机
    /// </summary>
    public string Phone { get; set; } = null!;

    [JsonIgnore]
    public string Salt { get;private set; } = null!;
    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Normal;

    public void SetPassword(string newPassword)
    {
        Salt = EntityId.NewId().ToString();
        var password = newPassword + Salt;
        Password = Md5Helper.ToHash(password);

    }
}