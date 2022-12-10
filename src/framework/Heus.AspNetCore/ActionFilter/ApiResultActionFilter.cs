using Heus.Core;
using Heus.Core.Common;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Heus.AspNetCore.ActionFilter;

internal class ApiResultActionFilter:IAsyncActionFilter,IScopedDependency
{
   

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var result= await next();
        if (context.Controller is IApplicationService && result.Result is ObjectResult objectResult )
        {
            objectResult.Value = ApiResult.Ok(objectResult.Value);
            context.Result = objectResult;
        }
        // context.Result = result.Result;
    }
}