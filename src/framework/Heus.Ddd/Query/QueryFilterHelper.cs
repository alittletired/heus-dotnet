using Heus.Ddd.Dtos;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Heus.Ddd.Query;
internal static class QueryFilterHelper
{
    private static ConcurrentDictionary<string, FilterMapping> _dynamicMappingCache = new();

    public static FilterMapping GetDynamicMappings(Type dtoType,  Type elementType)
    {
        var parameters = new Type[] { elementType };
        if (elementType.IsGenericType)
        {
            parameters=elementType.GetType().GetGenericArguments();
        }
        var key = dtoType.Name + ":" + parameters.Select(t => t.Name).JoinAsString(":");
        return _dynamicMappingCache.GetOrAdd(key, k =>
        {
            var mapping = new FilterMapping(dtoType, parameters);
            var dtoProps = dtoType.GetTypeInfo().DeclaredProperties.Where(p => p.CanWrite);
            foreach (var dtoProp in dtoProps)
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var entityType = parameters[i];
                    var entityProps = entityType.GetProperties();
                    var mappingProp = entityProps.FirstOrDefault(p => dtoProp.Name == p.Name ||
                                                                      dtoProp.Name == entityType.Name + p.Name);
                    if (mappingProp != null)
                    {
                        mapping.Mappings.Add(dtoProp.Name,new FilterMapping.MappingItem(dtoProp, mappingProp,entityType ,i));
                        break;
                    }
                }

            }
            return mapping;
        });
    }

   
}