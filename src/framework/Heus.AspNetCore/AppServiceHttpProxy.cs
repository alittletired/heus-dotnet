using System.Reflection;

namespace Heus.AspNetCore;

public class AppServiceHttpProxy<T> : DispatchProxy
{
    private IServiceProvider _serviceProvider = null!;

    private HttpClient _httpClient = null!;

    // protected AppServiceHttpProxy(){}
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        throw new NotImplementedException();
    }

    public static T Create(IServiceProvider serviceProvider, HttpClient httpClient)
    {
        var proxy = DispatchProxy.Create<T, AppServiceHttpProxy<T>>();
        if (proxy is AppServiceHttpProxy<T> httpProxy)
        {
            httpProxy.SetParameters(serviceProvider, httpClient);
        }
        else
        {
            throw new InvalidCastException("不应该执行到此处！");
        }

        return proxy;
    }

    private void SetParameters(IServiceProvider serviceProvider, HttpClient httpClient)
    {
        _serviceProvider = serviceProvider;
        _httpClient = httpClient;
    }
}