using System.Reflection;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Heus.AspNetCore.Conventions;

internal class ServiceControllerFeatureProvider:ControllerFeatureProvider
{
  protected override bool IsController(TypeInfo typeInfo)
  {
      var type = typeInfo.AsType();
      if (typeof(IApplicationService).IsAssignableFrom(typeInfo))
      {
        if (!typeInfo.IsInterface &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeInfo.IsPublic)
        {
          return true;
        }
      }

      return false;
  }
}