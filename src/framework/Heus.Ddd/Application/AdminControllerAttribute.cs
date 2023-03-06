namespace Heus.Ddd.Application;

public class AdminControllerAttribute:Attribute
{
    public AdminControllerAttribute(string code="", string name="", string description="")
    {
        Code = code;
        Name = name;
        Description = description;
    }

    public string Code { get;  }
    public string Name { get;  }
    public string Description { get;  }
}