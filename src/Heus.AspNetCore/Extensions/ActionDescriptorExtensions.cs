using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace  Microsoft.AspNetCore.Mvc.Filters;
public static class ActionDescriptorExtensions
{
   
    public static MethodInfo? GetMethodInfo(this ActionDescriptor actionDescriptor)
    {
        return (actionDescriptor as ControllerActionDescriptor)?.MethodInfo;
    }

    
   
    public static bool IsControllerAction(this ActionDescriptor actionDescriptor)
    {
        return actionDescriptor is ControllerActionDescriptor;
    }

    public static bool IsPageAction(this ActionDescriptor actionDescriptor)
    {
        return actionDescriptor is PageActionDescriptor;
    }
}


