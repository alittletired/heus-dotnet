
namespace Heus.Core.ObjectMapping;

[AttributeUsage(AttributeTargets.Class)]
public class TwoWaysMapAttribute:Attribute, IMapInfo
{
    public Type MappingType { get; }

    public MapType MapType =>MapType.TwoWay;

    public TwoWaysMapAttribute(Type type)
    {
        MappingType = type;
    }
}
