using System.Reflection;
using System.Text;
using Heus.Core.Utils;
using Heus.Ddd.Application;

namespace Heus.Core.Http;

public static class HttpApiHelper
{
    public static string GetGroupName(Type type)
    {
        if (type.IsAssignableTo<IManageService>())
            return "management";
        if (type.IsAssignableTo<IApplicationService>())
            return "api";
        return "";
    }

    public static string CalculateRouteTemplate(MethodInfo methodInfo)
    {
        var routeTemplate = new StringBuilder();
        if (methodInfo.DeclaringType?.IsAssignableTo<IManageService>() == true)
        {
            routeTemplate.Append("management");
        }
        else
        {
            routeTemplate.Append("api");
        }

        // 控制器名称部分
        var controllerName = methodInfo.DeclaringType!.Name;

        if (controllerName.EndsWith("ApplicationService"))
        {
            controllerName = controllerName[..^"ApplicationService".Length];
        }
        else if (controllerName.EndsWith("AppService"))
        {
            controllerName = controllerName[..^"AppService".Length];
        }
        else if (controllerName.EndsWith("ManagementService"))
        {
            controllerName = controllerName[..^"ManagementService".Length];
        }



        controllerName = PluralizerHelper.Pluralize(controllerName).ToKebabCase();
        routeTemplate.Append($"/{controllerName}");

        // id 部分
        if (methodInfo.GetParameters().Any(temp => temp.Name == "id"))
        {
            routeTemplate.Append("/{id}");
        }

        // Action 名称部分
        var actionName = methodInfo.Name;
        if (actionName.EndsWith("Async"))
        {
            actionName = actionName.Substring(0, actionName.Length - "Async".Length);
        }

        var trimPrefixes = new[]
        {
            "GetAll", "GetList", "Get",
            "Post", "Create", "Add", "Insert",
            "Put", "Update",
            "Delete", "Remove",
            "Patch"
        };
        if (char.IsLower(actionName[0]))
        {
            throw new BusinessException($"{methodInfo.DeclaringType!.Name} 不符合命名规范，必须是大写字母开头");

        }

        foreach (var trimPrefix in trimPrefixes)
        {
            if (!actionName.StartsWith(trimPrefix)) continue;
            actionName = actionName[trimPrefix.Length..];
            break;
        }

        if (!string.IsNullOrEmpty(actionName))
        {
            routeTemplate.Append($"/{actionName.ToCamelCase()}");
        }

        return routeTemplate.ToString();
    }

    public static HttpMethod GetHttpMethod(MethodInfo action)
    {
        var actionName = action.Name;
        if (actionName.StartsWith("Get"))
        {
            return HttpMethod.Get;
        }

        if (actionName.StartsWith("Put") || actionName.StartsWith("Update"))
        {
            return HttpMethod.Put;
        }

        if (actionName.StartsWith("Delete") || actionName.StartsWith("Remove"))
        {
            return HttpMethod.Delete;
        }

        if (actionName.StartsWith("Patch"))
        {
            return HttpMethod.Patch;
        }

        return HttpMethod.Post;
    }
}