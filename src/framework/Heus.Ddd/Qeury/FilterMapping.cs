using System.Reflection;
namespace Heus.Ddd.Qeury;
internal class FilterMapping
{
    internal class MappingItem
    {
        public MappingItem(PropertyInfo dtoProperty, PropertyInfo entityProperty, Type entityType, int paramIndex)
        {
            DtoProperty = dtoProperty;
            EntityProperty = entityProperty;
            ParamIndex = paramIndex;
            EntityType = entityType;
        }

        public PropertyInfo DtoProperty { get; }
        public PropertyInfo EntityProperty { get; }
        public int ParamIndex { get; }
        public Type EntityType { get; }
    }
    public FilterMapping(Type dtoType, params Type[] entityTypes)
    {
        DtoType = dtoType;
        EntityTypes = entityTypes;
    }

    public Type DtoType { get; }
    public Type[] EntityTypes { get; }
    public Dictionary<string, MappingItem> Mappings { get; } = new();
}