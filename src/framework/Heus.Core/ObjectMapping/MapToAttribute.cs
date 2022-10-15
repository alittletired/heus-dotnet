namespace Heus.Core.ObjectMapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapToAttribute : Attribute, IMapInfo
    {
        public MapToAttribute(Type type)
        {
            MappingType=type;
        }
        public Type MappingType { get; }

        public MapType MapType => MapType.MapTo;
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class MapFromAttribute : Attribute, IMapInfo
    {
        public MapFromAttribute(Type type)
        {
            MappingType = type;
        }
        public Type MappingType { get; }

        public MapType MapType => MapType.MapFrom;
    }
}
