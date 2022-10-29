using Heus.Core.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Heus.AspNetCore.Validation;

internal class ValidationActionFilter:IAsyncActionFilter,IScopedDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }
        context.HttpContext.RequestServices.GetRequiredService<IModelStateValidator>().Validate(context.ModelState);
        await next();
    }
}