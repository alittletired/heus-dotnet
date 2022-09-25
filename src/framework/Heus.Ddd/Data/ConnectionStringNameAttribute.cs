using System.Reflection;

namespace Heus.Ddd.Data;

public class ConnectionStringNameAttribute: Attribute
{
    public ConnectionStringNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public static string GetConnStringName<T>()
    {
        return GetConnStringName(typeof(T));
    }

    public static string GetConnStringName(Type type)
    {
        var nameAttribute = type.GetTypeInfo().GetCustomAttribute<ConnectionStringNameAttribute>();

        if (nameAttribute == null)
        {
            return type.Name;
        }

        return nameAttribute.Name;
    }
}