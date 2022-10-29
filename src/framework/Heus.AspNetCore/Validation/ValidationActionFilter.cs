using Microsoft.AspNetCore.Mvc.Filters;

namespace Heus.AspNetCore.Validation;

public class ValidationActionFilter:IAsyncActionFilter
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