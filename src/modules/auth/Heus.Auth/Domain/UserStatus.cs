namespace Heus.Auth.Domain;

/// <summary>
/// 用户状态
/// </summary>
public class UserStatus : EnumBase<UserStatus>
{
    protected UserStatus(string name, int value) : base(name, value)
    {
    }

    /// <summary>
    /// 正常
    /// </summary>
    public static readonly UserStatus Normal = new("正常", 0);

    /// <summary>
    /// 禁用
    /// </summary>
    public static readonly UserStatus Disabled = new("禁用", 1);

    /// <summary>
    /// 未激活
    /// </summary>
    public static readonly UserStatus Unactivated = new("未激活", 2);

    /// <summary>
    /// 锁定
    /// </summary>
    public static readonly UserStatus Locked = new("锁定", 3);

    /// <summary>
    /// 不存在
    /// </summary>
    public static readonly UserStatus NotFound = new("不存在", 4);
}