
using Heus.AspNetCore.TestBase;
using Heus.Core;
using Heus.Ddd.Domain;
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

    [Fact]
    public async Task Get_ThrowError()
    {
     var ex=   await Assert.ThrowsAsync<BusinessException>(() => _userAppService.GetAsync(-1));
     ex.Code.ShouldBe(404);
     

    }

    [Theory]
    [InlineData(nameof(User.Name),"",true)]
    [InlineData(nameof(User.Name),MockData.UserName1,true)]
    [InlineData(nameof(User.Phone),"notexist",false)]
    public async Task Search_Test(string propName,string value,bool hasResult)
    {
        var search = new DynamicSearch<User>();
        if (!string.IsNullOrEmpty(value))
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