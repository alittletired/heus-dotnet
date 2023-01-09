using Heus.Core.Common;
using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Microsoft.Net.Http.Headers;

namespace Heus.AspNetCore.ExceptionHandling;

public class ExceptionHandlingMiddleware : IMiddleware, IScopedDependency
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAndWrapException(context, ex);
        }
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        httpContext.Response.Clear();
        httpContext.Response.StatusCode = 200;
        httpContext.Response.OnStarting(ClearCacheHeaders, httpContext.Response);
        httpContext.Response.Headers.Add("Content-Type", "application/json");
        var error = JsonUtils.Serialize(ApiResult.FromException(exception));
        await httpContext.Response.WriteAsync(error);

    }

    private Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;

        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    }
}