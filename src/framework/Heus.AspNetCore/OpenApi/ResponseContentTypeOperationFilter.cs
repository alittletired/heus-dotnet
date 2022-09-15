namespace Heus.AspNetCore.OpenApi;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class ResponseContentTypeOperationFilter : IOperationFilter
{

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // if (!context.ApiDescription.TryGetMethodInfo(out var methodInfo))
        // {
        //     return;
        // }
        List<string> removeKeys = new List<string>() { "text/plain", "text/json", "application/*+json" };
        foreach (var res in operation.Responses.Values)
        {
            removeKeys.ForEach((key) => res.Content.Remove(key));
        }

        var reqBody = operation.RequestBody;
        if (reqBody != null)
        {
            removeKeys.ForEach((key) => reqBody.Content.Remove(key));
        }

    }
}
