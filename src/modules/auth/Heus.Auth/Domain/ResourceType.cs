namespace Heus.Auth.Domain;

/// <summary>
/// 资源类型
/// </summary>
public class ResourceType : EnumClass<ResourceType>
{
    protected ResourceType(string name, int value,string display) : base(name, value, display)
    {

    }

    /// <summary>
    /// 系统
    /// </summary>
    public static ResourceType Application = new(nameof(Application), 0, "系统");

    /// <summary>
    /// 菜单组
    /// </summary>
    public static ResourceType MenuGroup = new(nameof(MenuGroup) , 1, "菜单组");

    /// <summary>
    /// 菜单
    /// </summary>
    public static ResourceType Menu = new(nameof(Menu)  ,1,"菜单");

    /// <summary>
    /// 动作点
    /// </summary>
    public static ResourceType Action = new(nameof(Action), 1, "动作点");

}