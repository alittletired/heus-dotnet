namespace Heus.Auth.Domain;

/// <summary>
/// 资源类型
/// </summary>
public class ResourceType : EnumBase<ResourceType>
{
    private ResourceType(string name, int value) : base(name, value)
    {

    }

    /// <summary>
    /// 系统
    /// </summary>
    public static ResourceType Application = new("系统", 0);

    /// <summary>
    /// 菜单组
    /// </summary>
    public static ResourceType MenuGroup = new("菜单组", 1);

    /// <summary>
    /// 菜单
    /// </summary>
    public static ResourceType Menu = new("菜单", 1);

    /// <summary>
    /// 动作点
    /// </summary>
    public static ResourceType Action = new("动作点", 1);

}