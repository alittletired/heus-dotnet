using Heus.Core.DependencyInjection;
using Heus.Ddd.Uow;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Heus.AspNetCore.ActionFilter;

internal class UowActionFilter:IAsyncActionFilter,IScopedDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    public UowActionFilter(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }
        var methodInfo = context.ActionDescriptor.GetMethodInfo();
        var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);
        if (unitOfWorkAttr?.IsDisabled==true)
        {
            await next();
            return;
        }
      
        var options = CreateOptions(context, unitOfWorkAttr);
        using var uow = _unitOfWorkManager.Begin(options);
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
    private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute? unitOfWorkAttribute)
    {
        var options = new UnitOfWorkOptions();
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