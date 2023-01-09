using System.Collections;
using Heus.Core;
using Heus.Core.Http;
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
        var httpMethod = HttpMethodHelper.GetHttpMethod(action.ActionMethod);
        var routeTemplate = HttpApiHelper.CalculateRouteTemplate(action.Controller.ControllerType, action.ActionMethod);
        var routeAttr = new RouteAttribute(routeTemplate);
        var selector = new SelectorModel {
            AttributeRouteModel = new AttributeRouteModel(routeAttr),
            ActionConstraints = { new HttpMethodActionConstraint(new[] { httpMethod.ToString() }) }
        };

        action.Selectors.Add(selector);
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