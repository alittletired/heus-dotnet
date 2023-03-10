using Heus.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Heus.AspNetCore.Conventions;

public class ApiExplorerGroupConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        controller.ApiExplorer.IsVisible ??= true;

        foreach (var action in controller.Actions)
        {
            action.ApiExplorer.IsVisible ??= true;
            action.ApiExplorer.GroupName = HttpApiHelper.GetGroupName(action.Controller.ControllerType);
        }
    }

}