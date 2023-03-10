using Heus.Ddd.Application;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Heus.AspNetCore.Http;

public static class RemoteServiceClientExtensions
{
    public static void AddHttpClientProxy<T>(this IServiceCollection services,string remoteServiceName)
        where T:class,IApplicationService
    {
        services.TryAddSingleton<RemoteServiceProxyFactory>();
        services.AddSingleton(sp =>
        {
            var proxyFactory = sp.GetRequiredService<RemoteServiceProxyFactory>();
            return proxyFactory.CreateProxy<T>(remoteServiceName);
        });
    }
} 