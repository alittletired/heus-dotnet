namespace Heus.Business.Entities;


/// <summary>
/// 用户状态
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// 正常
    /// </summary>
    Normal,
    /// <summary>
    /// 禁用
    /// </summary>
    Disabled,
    /// <summary>
    /// 未激活
    /// </summary>
    Unactivated
}
public class User:EntityBase
{
    /// <summary>
    /// 用户账号
    /// </summary>
    public string Account { get; set; } = null!;
   /// <summary>
   /// 用户账号
   /// </summary>
    public string Password { get; set; } = null!;
   /// <summary>
   /// 用户手机
   /// </summary>
   public  string Phone { get; set; } = null!;
   /**
     * 密码盐
     */
   private String Salt{ get; set; } = null!;
   private UserStatus Status {get; set; }=UserStatus.Normal ;
}