using System.Globalization;
using Heus.Core.Security;

namespace Heus.AspNetCore.Authorization;

/// <summary>
/// 方便在开放环境时，调用api授权
/// </summary>
public class DevAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public DevAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var userName = context.Request.Headers["userName"];
     
        // if (userName.HasText())
        // {
        //  var userService=   context.RequestServices.GetRequiredService<IUserService>();      
        //  userService.FindByUserNameAsync()
        //
        // }

        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}