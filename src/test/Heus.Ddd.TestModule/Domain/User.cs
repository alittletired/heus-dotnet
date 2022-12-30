
using Heus.Core.Common;
using Heus.Ddd.Entities;

namespace Heus.Ddd.TestModule.Domain;

public class UserLevel: EnumClass<UserLevel>
{
    public UserLevel(string name, int value, string title) : base(name, value, title)
    {
    }

    public static UserLevel Level1 = new("level1", 1, "等级1");
    public static UserLevel Level2 = new("levle2", 2, "等级2");
}
public enum UserType
{
    /// <summary>
    /// 普通人
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 伟人
    /// </summary>
    GreatMan,
    Nobody
}
public class User : AuditEntity, ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public UserType UserType { get; set; }
    public int Sort { get; set; }
    public UserLevel UserLevel { get; set; } = UserLevel.Level1;

}
