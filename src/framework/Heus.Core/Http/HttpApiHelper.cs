using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Heus.Core.Utils;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Authorization;

namespace Heus.Core.Http;

public static class HttpApiHelper
{
    public static string GetGroupName(Type type)
    {

        if (type.IsAssignableTo<IAdminApplicationService>())
            return "admin";
        else if (type.IsAssignableTo<IApplicationService>())
            return "api";
        return "default";
    }
    //public static string GetAreaName(Type targetType)
    //{
    //    //targetType.Assembly
    //}

    public static string CalculateRouteTemplate(Type targetType, MethodInfo methodInfo)
    {
        var routeTemplate = new StringBuilder();
        if (targetType.IsAssignableTo<IAdminApplicationService>() == true)
        {
            routeTemplate.Append("admin");
        }
        else
        {
            routeTemplate.Append("api");
        }

        // 控制器名称部分
        var controllerName = targetType.Name;
        if (targetType.IsInterface && controllerName.StartsWith("I"))
        {
            controllerName = controllerName.Substring(1);
            if (targetType.IsGenericType) {
                throw new InvalidOperationException($"不支持泛型控制器,{targetType.FullName}");
                }
        }
        if (controllerName.EndsWith("ApplicationService"))
        {
            controllerName = controllerName[..^"ApplicationService".Length];
        }

        else if (controllerName.EndsWith("AdminAppService"))
        {
            controllerName = controllerName[..^"AdminAppService".Length];
        }
        else if (controllerName.EndsWith("AppService"))
        {
            controllerName = controllerName[..^"AppService".Length];
        }

        controllerName = HumanizerUtils.Kebaberize(controllerName);
        routeTemplate.Append($"/{controllerName}");

        // id 部分
        // if (methodInfo.GetParameters().Any(temp => temp.Name == "id"))
        // {
        //     routeTemplate.Append("/{id}");
        // }

        // Action 名称部分
        var actionName = methodInfo.Name;
        if (actionName.EndsWith("Async"))
        {
            actionName = actionName.Substring(0, actionName.Length - "Async".Length);
        }

        // var trimPrefixes = new[]
        // {
        //     "GetAll", "GetList", "Get",
        //     "Post", "Create", "Add", "Insert",
        //     "Put", "Update",
        //     "Delete", "Remove",
        //     "Patch"
        // };
        // if (char.IsLower(actionName[0]))
        // {
        //     throw new BusinessException($"{methodInfo.DeclaringType!.Name} 不符合命名规范，必须是大写字母开头");
        //
        // }
        //
        // foreach (var trimPrefix in trimPrefixes)
        // {
        //     if (!actionName.StartsWith(trimPrefix)) continue;
        //     actionName = actionName[trimPrefix.Length..];
        //     break;
        // }

        if (!string.IsNullOrEmpty(actionName))
        {
            routeTemplate.Append($"/{HumanizerUtils.Camelize(actionName)}");
        }

        return routeTemplate.ToString();
    }
    /// <summary>
    /// 是否允许匿名访问
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool IsAllowAnonymous(Type targetType, MethodInfo action)
    {
        return action.GetCustomAttribute<AllowAnonymousAttribute>() != null
               || targetType.GetCustomAttribute<AllowAnonymousAttribute>() != null;
    }
    public static HttpRequestMessage CreateHttpRequest(Type targetType, MethodInfo action, object?[]? args)
    {
        var routeTemplate = CalculateRouteTemplate(targetType,action);
        var httpMethod = HttpMethodHelper.GetHttpMethod(action);
        StringContent? content = null;

        var parameters = new Dictionary<string, object>();
        for (var i = 0; i < action.GetParameters().Length; i++)
        {
            var parameter = action.GetParameters()[i];
            var value = args?[i] ?? parameter.DefaultValue;
            if (value != null)
            {
                parameters.Add(parameter.Name!, value);
            }

        }

        var url = Regex.Replace(routeTemplate, @"\{([^\}]+)\}", evaluator =>
        {
            var parameter = parameters[evaluator.ToString()];
            parameters.Remove(evaluator.ToString());
            return parameter.ToString()!;
        });
        if (httpMethod == HttpMethod.Get)
        {
            if (parameters.Count > 0)
            {
                var queryString = parameters.Select(ConvertToQueryString).JoinAsString("&");
                url += $"?{queryString}";
            }

        }
        else if (parameters.Count > 1)
        {
            throw new BusinessException("无法解析多个body参数");
        }
        else if (parameters.Count == 1)
        {
            content = new StringContent(JsonUtils.Serialize(parameters.First().Value), Encoding.UTF8, MimeTypes.Application.Json);
        }
        var request = new HttpRequestMessage(httpMethod, new Uri(url,UriKind.Relative))
        {
            Content = content
        };
        return request;
    }

    private static string ConvertToQueryString(KeyValuePair<string, object> pair)
    {
        var valueType = pair.Value.GetType();
        if (valueType.IsClass && valueType != typeof(string))
        {

        }

        return $"{pair.Key}={pair.Value}";
    }
}