
using Heus.Ddd.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;


namespace Heus.AspNetCore.OpenApi;
public static class OpenApiExtensions
{
    public static void AddOpenApi(this IServiceCollection services, IHostEnvironment env)
    {
       
        services.AddSwaggerGen(c =>
        {
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
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json", env.ApplicationName));
    }
}
