using System.Xml;
using Heus.Ddd.Dtos;

namespace Heus.AspNetCore.OpenApi;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.OpenApi.Any;

// internal class OpenApiEntity : OpenApiString
// {
//     public OpenApiEntity(string value) : base(value)
//     {
//     }
//
//     public OpenApiEntity(string value, bool isExplicit) : base(value, isExplicit)
//     {
//     }
//     
// }
internal class EnumSchemaFilter : ISchemaFilter
{


    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // if (context.Type.IsGenericType && context.Type.GetGenericTypeDefinition() == typeof(DynamicQuery<>))
        // {
        //     var argumentType = context.Type.GetGenericArguments().First();
        //     var argumentSchema = context.SchemaGenerator.GenerateSchema(argumentType, context.SchemaRepository);
        //     var baseSchemaName = $"DynamicQuery<{argumentType.Name}>";
        //     var baseSchema = new OpenApiSchema()
        //     {
        //         Required = new SortedSet<string>() { "type" },
        //         Type = "object",
        //         Properties = new Dictionary<string, OpenApiSchema>
        //         {
        //             { "type", argumentSchema }
        //         }
        //     };
        //     context.SchemaRepository.AddDefinition(baseSchemaName, baseSchema);
        //     schema.Properties.Clear();
        //     // schema.Type = "object";
        //     schema.Reference = new OpenApiReference { Id = $"{baseSchemaName}", Type = ReferenceType.Schema };
        //     return;
        // }
      
        if (!context.Type.IsEnum) 
            return;
        var enums = schema.Enum;
        schema.Properties.Clear();
        schema.Enum = null;
        schema.Type = "object";
        schema.Format = "enum";
        var fieldSummaryDict = GetEnumSummary(context.Type);


        foreach (var enumValue in enums)
        {
            if (enumValue is not OpenApiPrimitive<int> openApiValue)
            {
                throw new Exception("枚举类型不能转化");
            }

            OpenApiSchema propSchema = new();
            var value = openApiValue.Value;
            var name = Enum.GetName(context.Type, value)!;
            var enumMember = context.Type.GetMember(name).First();
            propSchema.Default = new OpenApiInteger(value);

            if (fieldSummaryDict.TryGetValue(enumMember.Name, out var summary))
            {
                propSchema.Description = summary;
            }

            schema.Properties[name] = propSchema;
        }

    }

    private static Dictionary<string, string> GetEnumSummary(Type type)
    {
        var dict = new Dictionary<string, string>();
        var fieldPrefix = $"F:{type.FullName}.";
        var fileName = type.Assembly.Location;
        var xmlFile = fileName.Substring(0, fileName.Length - 4) + ".xml";
        if (!File.Exists(xmlFile))
        {
            Console.WriteLine($"{xmlFile} not exists");
            return dict;
        }
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
                        dict[name.Replace(fieldPrefix, "")] = sonNode.InnerText.Trim(' ','\n');
                    }
                }
            }
        }

        return dict;
    }
}
