
using Heus.AspNetCore.TestBase;

using Heus.Ddd.Dtos;
using Heus.Ddd.TestModule.Application;
using Heus.Ddd.TestModule.Domain;

namespace Heus.AspNetCore.Tests.Application;

public class PeopleAppServiceTests: WebIntegratedTestBase<AspNetWebApplicationFactory>
{
    private readonly IUserAdminAppService _userAppService;
    public PeopleAppServiceTests(AspNetWebApplicationFactory factory) : base(factory)
    {
        _userAppService = CreateServiceProxy<IUserAdminAppService>();
    }
  
    //private readonly IPeopleAppService _userAppService;
    //private readonly IRepository<User> _repository;
    //public PeopleAppServiceTests(AspNetWebApplicationFactory factory)
    //{
    //    _factory = factory;
    //    _repository = repository;
    //    _userAppService = _factory.GetServiceProxy<IPeopleAppService>();
    //}

    [Fact]
    public async Task Search_Test()
    {
        var search = new DynamicSearch<User>();
        var result = await _userAppService.SearchAsync(search);
        result.Total.ShouldBeGreaterThan(0);
        search.AddEqualFilter(s => s.Name, "test1");
        var result2 = await _userAppService.SearchAsync(search);
        result2.Total.ShouldBe(1);
         search.AddEqualFilter(s => s.Phone, "notexist");
        var result3 = await _userAppService.SearchAsync(search);
        result3.Total.ShouldBe(0);
    }

}