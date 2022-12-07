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
    
    public required string Name { get; init; } 
    /// <summary>
    /// 用户账号
    /// </summary>
    [JsonIgnore]
    public string Password { get; private set; } = null!;
    [JsonIgnore]
    public  string Salt { get; private set; } = null!;
    /// <summary>
    /// 用户手机
    /// </summary>
    public required string Phone { get; init; } = null!;
    public required string NickName { get; init; } = null!;
  
    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Normal;
    public bool IsSuperAdmin { get; set; }
    
    public void SetPassword(string plaintextPassword)
    {
        Salt = RandomUtils.GenerateString(10);
      
        var password = plaintextPassword + Salt;
        Password = Md5Utils.ToHash(password);
    }
}