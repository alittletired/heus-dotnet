using Heus.Core;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Heus.AspNetCore.Conventions;

public class ApiExplorerGroupConvention:IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        controller.ApiExplorer.IsVisible ??= true;

        foreach (var action in controller.Actions)
        {
            action.ApiExplorer.IsVisible ??= true;
            if (action.Controller.ControllerType.IsAssignableTo<IManagementService>())
                action.ApiExplorer.GroupName = "management";
        }
    }
}