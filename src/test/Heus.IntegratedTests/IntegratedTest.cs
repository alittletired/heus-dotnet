﻿using Heus.Core.DependencyInjection;
using Heus.Core.Http;

using Microsoft.AspNetCore.Mvc.Testing;


namespace Heus.IntegratedTests;

public class IntegratedTest<TStartup>: WebApplicationFactory<TStartup>
 where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ModuleCreateOptions.AdditionalModules.Add(typeof(IntegratedTestModuleInitializer));
        builder.UseEnvironment("Testing");
        base.ConfigureWebHost(builder);
    }
 
 
   
    public T GetServiceProxy<T>(string remoteServiceName) where T : IRemoteService
    {
        return Services.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<T>(remoteServiceName);
    }
}

