using System.Collections.Concurrent;
using System.Reflection;

namespace Heus.Ddd.Query;

static internal class QueryFilterHelper
{
    private readonly static ConcurrentDictionary<string, FilterMapping> DynamicMappingCache = new();

    public static FilterMapping GetDynamicMappings(Type dtoType, Type elementType)
    {
        var parameters = new[] { elementType };
        if (elementType.IsGenericType)
        {
            parameters = elementType.GetGenericArguments();
        }

        var key = dtoType.Name + ":" + parameters.Select(t => t.Name).JoinAsString(":");
        return DynamicMappingCache.GetOrAdd(key, _ =>
        {
            var mapping = new FilterMapping(dtoType, parameters);
            var dtoProps = dtoType.GetRuntimeProperties().Where(p => p.CanWrite);
            foreach (var dtoProp in dtoProps)
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var entityType = parameters[i];
                    var entityProps = entityType.GetProperties();
                    var mappingProp = entityProps.FirstOrDefault(p => dtoProp.Name == p.Name ||
                                                                      dtoProp.Name == entityType.Name + p.Name);
                    if (mappingProp == null)
                    {
                        continue;
                    }

                    mapping.Mappings.Add(dtoProp.Name,
                        new FilterMapping.MappingItem(dtoProp, mappingProp, entityType, i));
                    break;
                }

            }

            return mapping;
        });
    }


}