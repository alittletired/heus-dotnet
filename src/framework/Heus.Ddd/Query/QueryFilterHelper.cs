using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var dtoProps = dtoType.GetRuntimeProperties().Where(p => p.CanWrite || p.GetCustomAttribute<NotMappedAttribute>() == null);
            foreach (var dtoProp in dtoProps)
            {
                FilterMapping.MappingItem? mappingItem = null;
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

                    mappingItem = new FilterMapping.MappingItem(dtoProp, mappingProp, entityType);
                    break;
                }
                if (mappingItem == null)
                {
                    throw new InvalidOperationException($"无法定位属性映射关系type:{dtoType.Name},property:{dtoProp.Name}，如果明确不需要映射,添加NotMappedAttribute特性或者属性不可写");
                }
                mapping.Mappings.Add(dtoProp.Name, mappingItem);
            }

            return mapping;
        });
    }

}