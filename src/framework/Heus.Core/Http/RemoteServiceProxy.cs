using System.Reflection;
using Heus.Core;
using Heus.Core.Utils;

namespace Heus.Core.Http;

internal class RemoteServiceProxy : DispatchProxy
{
    internal RemoteServiceProxyFactory ProxyFactory { get; set; } = null!;
    internal Type ProxyType { get; set; } = null!;
    internal string RemoteServiceName{ get; set; } = null!;
    internal HttpClient HttpClient => ProxyFactory.GetHttpClient(RemoteServiceName);

    private  readonly MethodInfo _invokeAsyncGeneric = typeof(RemoteServiceProxy).GetTypeInfo()
        .DeclaredMethods.First(s => s.Name == nameof(InvokeAsync));

    private async Task<T?> InvokeAsync<T>(MethodInfo targetMethod, object?[]? args)
    {
        var request = HttpApiHelper.CreateHttpRequest(ProxyType,targetMethod, args);
        await ProxyFactory.PopulateRequestHeaders(request);
        var response = await HttpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new BusinessException($"请求失败! request:{request},response:{response}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var data = JsonUtils.Parse<ApiResult<T>>(content);
        if (data == null)
        {
            throw new BusinessException($"无法解析返回内容：{content},type:{typeof(ApiResult<T>)}");
        }

        if (data?.Code != 0)
        {
            throw new BusinessException(data?.Message!);
        }

        return data.Data;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);
        var returnType = targetMethod.ReturnType;
        ArgumentNullException.ThrowIfNull(returnType);
        if (!returnType.IsGenericType ||
            returnType.GetGenericTypeDefinition() != typeof(Task<>))
        {
            throw new BusinessException($"{targetMethod}必须是异步Task方法，并且有返回值");
        }

        var type = returnType.GenericTypeArguments[0];
        return _invokeAsyncGeneric.MakeGenericMethod(type).Invoke(this, new object?[] { targetMethod, args });

    }
}