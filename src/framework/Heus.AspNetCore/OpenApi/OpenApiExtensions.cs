
using System.Reflection;
using Heus.Core.Utils;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Heus.AspNetCore.OpenApi;
public static class OpenApiExtensions
{
    private static string SchemaIdSelector(Type modelType)
    {
        var name= TypeUtils.GetSimplifiedName(modelType);
        return name;
    }
    public static void AddOpenApi(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();

        services.AddSwaggerGen(c =>
        {
            c.DescribeAllParametersInCamelCase();
            c.CustomSchemaIds( SchemaIdSelector);
          
            foreach (var filePath in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
            {
                c.IncludeXmlComments(filePath);
            }
            c.OperationFilter<ResponseContentTypeOperationFilter>();
            c.OperationFilter<FromQueryModelFilter>();
            c.SchemaFilter<EnumSchemaFilter>();
            c.SchemaFilter<EnumClassSchemaFilter>();
            c.SchemaFilter<RecordTypeSchemaFilter>();
            var name = Assembly.GetEntryAssembly()?.GetName().Name ?? "heus";
            c.SwaggerDoc("v1", new OpenApiInfo { Title = name, Version = "v1" });
            // var operationFilters = FindFilterDescriptors<IOperationFilter>(services);
            // c.OperationFilterDescriptors.AddRange(operationFilters);
            //
            // var schemaFilters =FindFilterDescriptors<ISchemaFilter>(services);
            // c.SchemaFilterDescriptors.AddRange(schemaFilters);

        });
    }
    public static void UseOpenApi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var apiDescriptionGroups = app.ApplicationServices.GetRequiredService<IApiDescriptionGroupCollectionProvider>()
                .ApiDescriptionGroups.Items;
            foreach (var description in apiDescriptionGroups)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
            }
        });

    }
}
