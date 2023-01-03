using System.Reflection;
namespace Heus.Ddd.Query;
internal class MappingItem
{
    public MappingItem(PropertyInfo dtoProperty, PropertyInfo entityProperty, Type entityType)
    {
        DtoProperty = dtoProperty;
        EntityProperty = entityProperty;

        EntityType = entityType;
    }

    public PropertyInfo DtoProperty { get; }
    public PropertyInfo EntityProperty { get; }

    public Type EntityType { get; }
}
internal class FilterMapping
{
   
    public FilterMapping(Type dtoType, params Type[] entityTypes)
    {
        DtoType = dtoType;
        EntityTypes = entityTypes;
    }

    public Type DtoType { get; }
    public Type[] EntityTypes { get; }
    public Dictionary<string, MappingItem> Mappings { get; } = new(StringComparer.OrdinalIgnoreCase);
}