using System.Collections;
using System.Text;
using Heus.Core;
using Heus.Core.Utils;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Heus.AspNetCore.Conventions;

internal class ServiceApplicationModelConvention:IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            if (typeof(IApplicationService).IsAssignableFrom(controller.ControllerType))
            {
                ConfigureApplicationService(controller);
            }
        }
    }
    protected  void ConfigureApplicationService(ControllerModel controller )
    {
      
        ConfigureSelector(controller);
        ConfigureParameters(controller);
    }
  
    private void ConfigureParameters(ControllerModel controller)
    {
        foreach (var action in controller.Actions)
        {
            
            foreach (var parameter in action.Parameters)
            {
                if (parameter.BindingInfo != null)
                {
                    continue;
                }

                if ((parameter.ParameterType.IsClass|| parameter.ParameterType.IsAssignableTo(typeof(IEnumerable)) )&&
                    parameter.ParameterType != typeof(string) &&
                    parameter.ParameterType != typeof(IFormFile))
                {
                    var httpMethods = action.Selectors.SelectMany(temp => temp.ActionConstraints)
                        .OfType<HttpMethodActionConstraint>().SelectMany(temp => temp.HttpMethods).ToList();
                    if (httpMethods.Contains("GET") ||
                        httpMethods.Contains("DELETE") ||
                        httpMethods.Contains("TRACE") ||
                        httpMethods.Contains("HEAD"))
                    {
                        continue;
                    }

                    parameter.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                }
            }
        }
    }
    private void ConfigureSelector(ControllerModel controller)
    {
        RemoveEmptySelectors(controller.Selectors);

        if (controller.Selectors.Any(temp => temp.AttributeRouteModel != null))
        {
            return;
        }

        foreach (var action in controller.Actions)
        {
            var method = action.ActionMethod;
            // if (!method.IsVirtual)
            // {
            //     throw new BusinessException(
            //         $"{action.ActionMethod.DeclaringType?.FullName}.{action.ActionMethod.Name} 必须定义为虚方法");
            // }

            if (!method.ReturnType.IsAssignableTo(typeof(Task)))
            {
                throw new BusinessException(
                    $"{action.ActionMethod.DeclaringType?.FullName}.{action.ActionMethod.Name} 必须为异步方法");
            }

            ConfigureSelector(action);
        }
    }
    private void ConfigureSelector(ActionModel action)
    {
        RemoveEmptySelectors(action.Selectors);

        if (action.Selectors.Count <= 0)
        {
            AddApplicationServiceSelector(action);
        }
        else
        {
            NormalizeSelectorRoutes(action);
        }
    }
    private void AddApplicationServiceSelector(ActionModel action)
    {
        var httpMethod = GetHttpMethod(action);
        var routeAttr = new RouteAttribute(CalculateRouteTemplate(action));
        var selector =  new SelectorModel
        {
            AttributeRouteModel = new AttributeRouteModel(routeAttr),
            ActionConstraints = { new HttpMethodActionConstraint(new[] { httpMethod }) }
        };

        action.Selectors.Add(selector);
    }
    private string CalculateRouteTemplate(ActionModel action)
    {
        var routeTemplate = new StringBuilder();
        if (action.ActionMethod.DeclaringType?.IsAssignableTo<IManagementService>() == true)
        {
            routeTemplate.Append("management");
        }
        else
        {
            routeTemplate.Append("api");
        }
        // 控制器名称部分
        var controllerName = action.Controller.ControllerName;
        
        if (controllerName.EndsWith("ApplicationService"))
        {
            controllerName = controllerName[..^"ApplicationService".Length];
        }
        else if (controllerName.EndsWith("AppService"))
        {
            controllerName = controllerName[..^"AppService".Length];
        }else if (controllerName.EndsWith("ManagementService"))
        {
            controllerName = controllerName[..^"ManagementService".Length];
        }

       

        controllerName = PluralizerHelper.Pluralize(controllerName).ToKebabCase();
        routeTemplate.Append($"/{controllerName}");

        // id 部分
        if (action.Parameters.Any(temp => temp.ParameterName == "id"))
        {
            routeTemplate.Append("/{id}");
        }

        // Action 名称部分
        var actionName = action.ActionName;
        if (actionName.EndsWith("Async"))
        {
            actionName = actionName.Substring(0, actionName.Length - "Async".Length);
        }
        var trimPrefixes = new[]
        {
            "GetAll","GetList","Get",
            "Post","Create","Add","Insert",
            "Put","Update",
            "Delete","Remove",
            "Patch"
        };
        if (char.IsLower(actionName[0]))
        {
            throw new BusinessException($"{action.Controller.ControllerName} 不符合命名规范，必须是大写字母开头");

        }
        foreach (var trimPrefix in trimPrefixes)
        {
            if (actionName.StartsWith(trimPrefix))
            {
                actionName = actionName.Substring(trimPrefix.Length);
                break;
            }
        }
        if (!string.IsNullOrEmpty(actionName))
        {
            routeTemplate.Append($"/{actionName.ToCamelCase()}");
        }

        return routeTemplate.ToString();
    }
    private string GetHttpMethod(ActionModel action)
    {
        var actionName = action.ActionName;
        if (actionName.StartsWith("Get"))
        {
            return "GET";
        }

        if (actionName.StartsWith("Put") || actionName.StartsWith("Update"))
        {
            return "PUT";
        }

        if (actionName.StartsWith("Delete") || actionName.StartsWith("Remove"))
        {
            return "DELETE";
        }

        if (actionName.StartsWith("Patch"))
        {
            return "PATCH";
        }

        return "POST";
    }
    
    private void NormalizeSelectorRoutes(ActionModel action)
    {
        foreach (var selector in action.Selectors)
        {
            selector.AttributeRouteModel ??= new AttributeRouteModel(new RouteAttribute(CalculateRouteTemplate(action)));

            if (selector.ActionConstraints.OfType<HttpMethodActionConstraint>()
                    .FirstOrDefault()?.HttpMethods.FirstOrDefault() == null)
            {
                selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { GetHttpMethod(action) }));
            }
        }
    }
   
  


    private void RemoveEmptySelectors(IList<SelectorModel> selectors)
    {
        for (var i = selectors.Count - 1; i >= 0; i--)
        {
            var selector = selectors[i];
            if (selector.AttributeRouteModel == null &&
                (selector.ActionConstraints.Count <= 0) &&
                (selector.EndpointMetadata.Count <= 0))
            {
                selectors.Remove(selector);
            }
        }
    }
  
}