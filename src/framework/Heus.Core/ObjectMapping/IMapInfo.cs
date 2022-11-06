namespace Heus.Core.ObjectMapping;
public enum MapType
{
    TwoWay,
    MapFrom,
    MapTo
}


public interface IMapInfo
{
    Type MappingType { get; }
    MapType MapType { get; }

}

