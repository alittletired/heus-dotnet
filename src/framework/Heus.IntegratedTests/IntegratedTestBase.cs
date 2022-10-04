using System.Reflection;
using Heus.AspNetCore;
using Heus.Ddd.Application;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Heus.IntegratedTests;

public abstract class IntegratedTestBase<TProgram,TService>:WebApplicationFactory<TProgram>
    where TProgram:class where TService:IRemoteService
{
    protected readonly HttpClient _httpClient;
    protected readonly TService _appService;
    public IntegratedTestBase()
    {
      
        _httpClient= CreateClient();
        _appService = AppServiceHttpProxy<TService>.Create(Services,_httpClient); 

    }
}