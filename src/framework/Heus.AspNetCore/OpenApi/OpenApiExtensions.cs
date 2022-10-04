
using Heus.Core.Utils;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Heus.AspNetCore.OpenApi;
public static class OpenApiExtensions
{
    private static string SchemaIdSelector(Type modelType)
    {
        var name= TypeHelper.GetSimplifiedName(modelType);
        return name;
    }
    public static void AddOpenApi(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();

        services.AddSwaggerGen(c =>
        {
            c.DescribeAllParametersInCamelCase();
            c.CustomSchemaIds( SchemaIdSelector);
            c.MapType<EntityId>(() => new OpenApiSchema { 
                Type = "string" 
                ,Example =new OpenApiString(EntityId.NewId().ToString()) });
            foreach (var filePath in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
            {
                try
                {
                    c.IncludeXmlComments(filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            c.OperationFilter<ResponseContentTypeOperationFilter>();
            c.OperationFilter<FromQueryModelFilter>();
            c.SchemaFilter<EnumSchemaFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = env.ApplicationName, Version = "v1" });
            // var operationFilters = FindFilterDescriptors<IOperationFilter>(services);
            // c.OperationFilterDescriptors.AddRange(operationFilters);
            //
            // var schemaFilters =FindFilterDescriptors<ISchemaFilter>(services);
            // c.SchemaFilterDescriptors.AddRange(schemaFilters);

        });
    }
    public static void UseOpenApi(this IApplicationBuilder app, IHostEnvironment env)
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
