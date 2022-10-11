namespace Heus.Auth.Domain;

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
    Unactivated,
    /// <summary>
    /// 锁定
    /// </summary>
    Locked,
    /// <summary>
    /// 不存在
    /// </summary>
    NotFound
}