using Heus.Core.Common;
using Heus.Ddd.Entities;

namespace Heus.AspNetCore.TestApp.Domain;

public class PersonEnum : EnumClass<PersonEnum>
{
    public PersonEnum(string name, int value, string title) : base(name, value, title)
    {
    }

    public static PersonEnum One = new("one", 1, "one:1");
    public static PersonEnum Two = new("two", 2, "two:2");
}
public enum PersonType
{
    /// <summary>
    /// 普通人
    /// </summary>
    Normal=0,
    /// <summary>
    /// 伟人
    /// </summary>
    GreatMan,
    Nobody
}
public class Person:AuditEntity,ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public PersonType Type { get; set; }
    public PersonEnum PersonEnum { get; set; } = PersonEnum.One;
}
