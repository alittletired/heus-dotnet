using Heus.DDD.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Heus.AspNetCore.ActionFilter;

internal class UowActionFilter:IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var methodInfo = context.ActionDescriptor.GetMethodInfo();
        var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);
        if (unitOfWorkAttr == null)
        {
            await next();
            return;
        }
        var unitOfWorkManager = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWorkManager>();
        var options = CreateOptions(context, unitOfWorkAttr);
        using var uow = unitOfWorkManager.Begin(options);
        var result = await next();
        if (Succeed(result))
        {
            await uow.CompleteAsync(context.HttpContext.RequestAborted);
        }
        else
        {
            await uow.RollbackAsync(context.HttpContext.RequestAborted);
        }
    }
    private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute unitOfWorkAttribute)
    {
        var options = new UnitOfWorkOptions(context.HttpContext.RequestServices);
        unitOfWorkAttribute?.SetOptions(options);
      
        if (unitOfWorkAttribute?.IsTransactional == null)
        {
            options.IsTransactional =  !string.Equals(context.HttpContext.Request.Method
                , HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase);
        }

        return options;
    }
    private static bool Succeed(ActionExecutedContext result)
    {
        return result.Exception == null || result.ExceptionHandled;
    }
}