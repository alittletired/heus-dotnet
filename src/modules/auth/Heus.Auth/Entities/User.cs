using Heus.Auth.Domain;
using Heus.Core.Utils;

namespace Heus.Auth.Entities;

using Heus.Core.ObjectMapping;
using Heus.Core.Security;
using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("auth_user")]
[MapTo(typeof(ICurrentUser))]
public class User : AuditEntity
{
    /// <summary>
    /// 用户账号
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;
    /// <summary>
    /// 用户账号
    /// </summary>
    [JsonIgnore]
    public string Password { get; private set; } = null!;
    /// <summary>
    /// 用户手机
    /// </summary>
    public string Phone { get; set; } = null!;
    public string NickName { get; set; } = null!;
    [JsonIgnore]
    public string Salt { get; private set; } = null!;
    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Normal;
    public bool IsSuperAdmin { get; set; }
    public void SetPassword(string newPassword)
    {
        Salt = RandomUtils.GenerateString(10);
        var password = newPassword + Salt;
        Password = Md5Utils.ToHash(password);
    }
}