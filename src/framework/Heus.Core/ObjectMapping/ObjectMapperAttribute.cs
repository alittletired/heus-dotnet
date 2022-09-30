
namespace Heus.Core.ObjectMapping;

[AttributeUsage(AttributeTargets.Class)]
public class ObjectMapperAttribute:Attribute
{
    public Type MappingType { get; }
    public ObjectMapperAttribute(Type type)
    {
        MappingType = type;
    }
}
