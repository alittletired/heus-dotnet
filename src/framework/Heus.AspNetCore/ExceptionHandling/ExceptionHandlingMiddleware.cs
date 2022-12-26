using Heus.Core.Common;
using Heus.Core.Utils;
using Microsoft.Net.Http.Headers;

namespace Heus.AspNetCore.ExceptionHandling;

public class ExceptionHandlingMiddleware:IMiddleware
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
            // We can't do anything if the response has already started, just abort.
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("An exception occurred, but response has already started!");
                throw;
            }

            await HandleAndWrapException(context, ex);
        }
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception,exception.Message);

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = 200;
            httpContext.Response.OnStarting(ClearCacheHeaders, httpContext.Response);
            httpContext.Response.Headers.Add("Content-Type", "application/json");
            await httpContext.Response.WriteAsync(JsonUtils.Serialize(ApiResult.Error(exception)));
        
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