using Heus.Ddd.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using static Heus.Ddd.Qeury.FilterMapping;

namespace Heus.Ddd.Qeury;
internal static class QueryFilterHelper
{
    private static ConcurrentDictionary<string, FilterMapping> _dynamicMappingCache = new();

    public static FilterMapping GetDynamicMappings(Type dtoType, IEnumerable<ParameterExpression> parameters)
    {
        var entityTypes = parameters.Select(p => p.Type).ToArray();
        var key = dtoType.Name + ":" + entityTypes.Select(t => t.Name).JoinAsString(":");
        return _dynamicMappingCache.GetOrAdd(key, k =>
        {
            var mapping = new FilterMapping(dtoType, entityTypes);
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
                        mapping.Mappings.Add(dtoProp.Name,new MappingItem(dtoProp, mappingProp,entityType ,i));
                        break;
                    }
                }

            }
            return mapping;
        });
    }

   public static List<QueryFilterItem> GetQueryFilterItems<T>(IQueryDto<T> queryDto)
    {
        if(queryDto is DynamicQuery<T> dynamicQuery)
        {
            return GetQueryFilterItems(dynamicQuery);
        }
        return new List<QueryFilterItem>();
     
    }
    public static List<QueryFilterItem> GetQueryFilterItems<T>(DynamicQuery<T> queryDto)
    {
        var filterItems = new List<QueryFilterItem>();
        foreach (var pair in queryDto.Filters)
        {
            if (pair.Value == null)
                continue;
            var val = pair.Value;
            var op = OperatorTypes.Equal;
            var alias = "";
         if(val is IDictionary<string,object> dict)
            {
                op = dict.GetOrDefault("op", op).ToString()!;
                alias = dict.GetOrDefault("alias", "").ToString();
                val = dict["value"];
            }
            filterItems.Add(new QueryFilterItem(pair.Key, op, val, alias));


        }
        return filterItems;
    }
}