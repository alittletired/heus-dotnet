using System.Collections;
using Heus.AspNetCore.Http;
using Heus.Core;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Heus.AspNetCore.Conventions;

internal class ServiceApplicationModelConvention : IApplicationModelConvention
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

    private void ConfigureApplicationService(ControllerModel controller)
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

                if ((parameter.ParameterType.IsClass || parameter.ParameterType.IsAssignableTo(typeof(IEnumerable))) &&
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
       

            NormalizeSelectorRoutes(action);
        
    }

    


    private void NormalizeSelectorRoutes(ActionModel action)
    {
        foreach (var selector in action.Selectors)
        {
            var routeTemplate =
                HttpApiHelper.CalculateRouteTemplate(action.Controller.ControllerType, action.ActionMethod);
            selector.AttributeRouteModel ??= new AttributeRouteModel(new RouteAttribute(routeTemplate));

            if (selector.ActionConstraints.OfType<HttpMethodActionConstraint>()
                    .FirstOrDefault()?.HttpMethods.FirstOrDefault() == null)
            {
                selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] {
                    HttpMethodHelper.GetHttpMethod(action.ActionMethod).ToString()
                }));
            }
        }
    }

   

}