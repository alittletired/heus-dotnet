namespace Heus.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.OpenApi.Any;
using Heus.Core;
using System.Reflection;

internal class EnumClassSchemaFilter : ISchemaFilter
{


    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
      

        if (!context.Type.IsAssignableTo(typeof(IEnumClass))) 
            return;
      
        schema.Properties.Clear();
        schema.Enum = null;
        schema.Type = "object";
        schema.Format = "enum";

        var enumClass = typeof(EnumClass<>).MakeGenericType(context.Type);
        var getEnumOptions = enumClass.GetTypeInfo().GetRuntimeMethods().First(s => s.Name == "GetEnumOptions");
        var options = getEnumOptions.Invoke(null,new object[]{ }) as IEnumClass[];
       

        foreach (var option in options!)
        {
            OpenApiSchema propSchema = new();
            var value = option.Value;
            var name = option.Name;
            propSchema.Default = new OpenApiInteger(value);
            propSchema.Title = option.Title;
            propSchema.Type = "integer";
            schema.Properties[name] = propSchema;
        }

    }

  
}
