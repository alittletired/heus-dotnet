
namespace Heus.Core.ObjectMapping;

[AttributeUsage(AttributeTargets.Class)]
public class TwoWayMapAttribute:Attribute, IMapInfo
{
    public Type MappingType { get; }

    public MapType MapType =>MapType.TwoWay;

    public TwoWayMapAttribute(Type type)
    {
        MappingType = type;
    }
}
