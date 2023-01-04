
using Heus.AspNetCore.TestBase;

using Heus.Ddd.Dtos;
using Heus.Ddd.Query;
using Heus.Ddd.TestModule;
using Heus.Ddd.TestModule.Application;
using Heus.Ddd.TestModule.Domain;

namespace Heus.AspNetCore.Tests.Application;

public class PeopleAppServiceTests:AspNetIntegratedTest
{
    private readonly IUserAdminAppService _userAppService;
    public PeopleAppServiceTests(WebApplicationFactory<AspNetCoreTestModule, Program> factory) : base(factory)
    {
        _userAppService = CreateServiceProxy<IUserAdminAppService>();
    }
   
   
    [Theory]
    [InlineData(nameof(User.Name),"",false)]
    [InlineData(nameof(User.Name),MockData.UserName1,true)]
    [InlineData(nameof(User.Phone),"notexist",false)]
    public async Task Search_Test(string propName,string value,bool hasResult)
    {
        var search = new DynamicSearch<User>();
        if (string.IsNullOrEmpty(value))
        {
            search.AddFilter(propName, OperatorTypes.Equal, value);     
        }
       
        var result = await _userAppService.SearchAsync(search);
        if (hasResult)
        {
            result.Total.ShouldBeGreaterThan(0);     
        }
        else
        {
            result.Total.ShouldBe(0);   
        }
       
    
       
    }

   
}