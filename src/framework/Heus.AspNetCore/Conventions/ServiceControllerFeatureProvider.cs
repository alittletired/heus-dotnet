using System.Reflection;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Heus.AspNetCore.Conventions;

internal class ServiceControllerFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        return !typeInfo.IsInterface &&
               !typeInfo.IsAbstract &&
               !typeInfo.IsGenericType &&
               typeof(IApplicationService).IsAssignableFrom(typeInfo);
    }
}