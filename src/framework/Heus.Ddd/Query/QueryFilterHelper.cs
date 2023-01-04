using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Heus.Ddd.Query;

static internal class QueryFilterHelper
{
    private static readonly ConcurrentDictionary<string, FilterMapping> DynamicMappingCache = new();
    public static readonly MethodInfo WhereMethodInfo = typeof(Queryable).GetRuntimeMethods()
        .First(s => s.Name == nameof(Queryable.Where) && s.GetParameters().Length == 2);
    public static readonly MethodInfo SelectMethodInfo = typeof(Queryable).GetRuntimeMethods()
        .First(s => s.Name == nameof(Queryable.Select) && s.GetParameters().Length == 2);
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
            var dtoProps = dtoType.GetRuntimeProperties().Where(p => p.CanWrite || p.GetCustomAttribute<NotMappedAttribute>() == null).Reverse();
            foreach (var dtoProp in dtoProps)
            {
                MappingItem? mappingItem = null;
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

                    mappingItem = new MappingItem(dtoProp, mappingProp, entityType);
                    break;
                }
                if (mappingItem == null)
                {
                    throw new InvalidOperationException($"�޷���λ����ӳ���ϵtype:{dtoType.Name},property:{dtoProp.Name}�������ȷ����Ҫӳ��,���NotMappedAttribute���Ի������Բ���д");
                }
                mapping.Mappings.Add(dtoProp.Name, mappingItem);
            }

            return mapping;
        });
    }

}