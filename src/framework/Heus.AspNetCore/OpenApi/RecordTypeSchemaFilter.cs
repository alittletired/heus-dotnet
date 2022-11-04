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

internal class RecordTypeSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!TypeHelper.IsRecordType(context.Type))
            return;
        var properties = context.Type.GetProperties();
        foreach (var p in properties)
        {
            if (!TypeHelper.IsNullable(p.PropertyType))
            {
                schema.Required.Add(p.Name.ToCamelCase());
            }

        }

    }


}
