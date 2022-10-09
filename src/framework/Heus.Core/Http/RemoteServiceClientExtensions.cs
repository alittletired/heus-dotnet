using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Heus.Core.Http;

public static class RemoteServiceClientExtensions
{
    public static void AddHttpClientProxy<T>(this IServiceCollection services,string remoteServiceName) where T:class,IRemoteService
    {
        services.TryAddSingleton<RemoteServiceProxyFactory>();
        services.AddSingleton(sp =>
        {
            var proxyFactory = sp.GetRequiredService<RemoteServiceProxyFactory>();
            return proxyFactory.CreateProxy<T>(remoteServiceName);
        });
    }
} 