namespace Heus.Auth.Domain;

/// <summary>
/// 用户状态
/// </summary>
public class UserStatus : EnumClass<UserStatus>
{
    protected UserStatus(string name, int value,string display) : base(name, value,  display)
    {
    }

    /// <summary>
    /// 正常
    /// </summary>
    public static readonly UserStatus Normal = new(nameof(Normal),  0, "正常");

    /// <summary>
    /// 禁用
    /// </summary>
    public static readonly UserStatus Disabled = new(nameof(Disabled), 1, "禁用");

    /// <summary>
    /// 未激活
    /// </summary>
    public static readonly UserStatus Unactivated = new(nameof(Unactivated), 2, "未激活");

    /// <summary>
    /// 锁定
    /// </summary>
    public static readonly UserStatus Locked = new(nameof(Locked) , 3, "锁定");

    /// <summary>
    /// 不存在
    /// </summary>
    public static readonly UserStatus NotFound = new(nameof(NotFound), 4, "不存在");
}