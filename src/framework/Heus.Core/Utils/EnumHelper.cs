using System.Collections.Concurrent;
using System.Xml;
using System;

namespace Heus.Core.Utils;

public static class EnumHelper
{
    private  readonly static ConcurrentDictionary<Type, Dictionary<string, string>> EnumSummaries= new();

   
    public static string GetSummary<TEnum>(TEnum enumValue) where TEnum: Enum
    {
     
        var enumSummaries = EnumSummaries.GetOrAdd(typeof(TEnum), GetEnumSummaries);
        if (enumSummaries.TryGetValue(enumValue.ToString(), out var summary))
        {
            return summary;
        }

        return enumValue.ToString();
    }
    private  static Dictionary<string, string> GetEnumSummaries(Type type) {
        var dict = new Dictionary<string, string>();
        var fieldPrefix = $"F:{type.FullName}.";
        var fileName = type.Assembly.Location;
        var xmlFile = string.Concat(fileName.AsSpan(0, fileName.Length - 4), ".xml");
      
        var doc = new XmlDocument();
        doc.Load(xmlFile);
        var nodes = doc.DocumentElement!.SelectNodes("//members/member")!;
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i]!;
            var name = node.Attributes?["name"]?.Value;
            if (node.HasChildNodes && name != null && name.StartsWith(fieldPrefix))
            {
                for (var j = 0; j < node.ChildNodes.Count; j++)
                {
                    var sonNode = node.ChildNodes[j]!;
                    if (sonNode.Name == "summary")
                    {
                        dict[name.Replace(fieldPrefix, "")] = sonNode.InnerText.Trim(' ','\r','\n');
                    }
                }
            }
        }

        return dict;
    }
}