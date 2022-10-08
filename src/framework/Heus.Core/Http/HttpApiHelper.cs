using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Heus.Core.Utils;
using Heus.Ddd.Application;

namespace Heus.Core.Http;

public static class HttpApiHelper
{
    public static string GetGroupName(Type type)
    {
      
        if (type.IsAssignableTo<IAdminApplicationService>())
          return "admin";
        else if (type.IsAssignableTo<IApplicationService>())
           return  "api";
        return "default";
    }

    public static string CalculateRouteTemplate(MethodInfo methodInfo)
    {
        var routeTemplate = new StringBuilder();
        if (methodInfo.DeclaringType?.IsAssignableTo<IAdminApplicationService>() == true)
        {
            routeTemplate.Append("admin");
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
      
        else if (controllerName.EndsWith("AdminAppService"))
        {
            controllerName = controllerName[..^"AdminAppService".Length];
        }
        else if (controllerName.EndsWith("AppService"))
        {
            controllerName = controllerName[..^"AppService".Length];
        }

        if (methodInfo.DeclaringType.IsInterface && controllerName.StartsWith("I"))
        {
            controllerName = controllerName[1..];
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

   
    public static HttpRequestMessage CreateHttpRequest(MethodInfo action, object?[]? args)
    {
        var routeTemplate = CalculateRouteTemplate(action);
        var httpMethod = GetHttpMethod(action);
        StringContent? content=null;
       
        var parameters = new Dictionary<string, object>();
        for(var i=0;i<action.GetParameters().Length ; i++)
        {
            var parameter = action.GetParameters()[i];
            var value = args?[i]?? parameter.DefaultValue;
            if(value!=null)
            {
                parameters.Add(parameter.Name!,value);
            }

        }
        
        var url= Regex.Replace(routeTemplate, @"\{([^\}]+)\}", evaluator =>
        {
            var parameter= parameters[evaluator.ToString()];
            parameters.Remove(evaluator.ToString());
            return parameter.ToString()!;
        });
        if (httpMethod == HttpMethod.Get  )
        {
            if (parameters.Count > 0)
            {
                var queryString = parameters.Select(ConvertToQueryString).JoinAsString("&");
                url += "?queryString";     
            }
           
        }else if (parameters.Count > 1)
        {
            throw new BusinessException("无法解析多个body参数");
        }else if (parameters.Count == 1)
        {
           content=  new StringContent(JsonUtils.Stringify(parameters.First().Value), Encoding.UTF8, MimeTypes.Application.Json);
        }
        var request = new HttpRequestMessage(httpMethod, new Uri(url))
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