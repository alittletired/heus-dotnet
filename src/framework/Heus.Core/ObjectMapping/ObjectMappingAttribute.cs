namespace Heus.Core.ObjectMapping;
[AttributeUsage(AttributeTargets.Class)]
public  class ObjectMappingAttribute : Attribute, IMapInfo
{
    public Type MappingType { get; }
    public MapType MapType { get; }

    public ObjectMappingAttribute(Type type, MapType mapType)
    {
        MappingType=type;
        MapType = mapType;
    }
}

