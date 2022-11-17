using System.Xml;
using Heus.Ddd.Dtos;

namespace Heus.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.OpenApi.Any;
using Heus.Core;
using System.Reflection;
using Heus.Core.Utils;
using System.Diagnostics.CodeAnalysis;

internal class RecordTypeSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!TypeUtils.IsRecordType(context.Type))
            return;
        var properties = context.Type.GetProperties();
        foreach (var p in properties)
        {
            if (p.Name == "Actions")
            {
                var type = p.PropertyType;
            }
          
            if ( !TypeUtils.IsNullable(p))
            {
                schema.Required.Add(p.Name.ToCamelCase());
            }

        }

    }


}
