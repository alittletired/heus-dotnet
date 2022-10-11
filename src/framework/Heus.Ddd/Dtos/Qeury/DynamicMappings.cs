using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Heus.Ddd.Dtos.Qeury;

internal class DynamicMappingItem
{
    public DynamicMappingItem(PropertyInfo dtoProperty, PropertyInfo entityProperty,Type entityType, int paramIndex)
    {
        DtoProperty = dtoProperty;
        EntityProperty = entityProperty;
        ParamIndex = paramIndex;
        EntityType = entityType;
    }

    public   PropertyInfo DtoProperty { get; }
 public PropertyInfo EntityProperty { get; }
 public int ParamIndex { get; }
 public Type EntityType{ get; }
}
internal class DynamicMapping
{
    public DynamicMapping(Type dtoType,params Type[] entityTypes)
    {
        DtoType = dtoType;
        EntityTypes = entityTypes;
    }

    public Type DtoType { get;  }
    public Type[] EntityTypes{ get;  }
    public Dictionary<string, DynamicMappingItem> Mappings { get;  } = new();
}

internal static class DynamicMappingsHelper
{
    private static ConcurrentDictionary<string, DynamicMapping> _dynamicMappingCache = new();

  

    public static DynamicMapping GetDynamicMappings(Type dtoType, IEnumerable<ParameterExpression> parameters)
    {
        var entityTypes = parameters.Select(p => p.Type).ToArray();
        var key = dtoType.Name + ":" + entityTypes.Select(t => t.Name).JoinAsString(":");
        return _dynamicMappingCache.GetOrAdd(key, k =>
        {
            var mapping = new DynamicMapping(dtoType, entityTypes);
            var dtoProps = dtoType.GetTypeInfo().DeclaredProperties.Where(p => p.CanWrite);
            foreach (var dtoProp in dtoProps)
            {
                for (var i = 0; i < entityTypes.Length; i++)
                {
                    var entityType = entityTypes[i];
                    var entityProps = entityType.GetProperties();
                    var mappingProp = entityProps.FirstOrDefault(p => dtoProp.Name == p.Name ||
                                                                      dtoProp.Name == entityType.Name + p.Name);
                    if (mappingProp != null)
                    {
                        mapping.Mappings.Add(dtoProp.Name,new DynamicMappingItem(dtoProp, mappingProp,entityType ,i));
                        break;
                    }
                }

            }
            return mapping;
        });
    }

   
}