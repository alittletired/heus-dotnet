using Heus.AspNetCore.TestApp.Application;
using Heus.AspNetCore.TestApp.Domain;
using Heus.AspNetCore.TestBase;
using Heus.Core.Security;
using Heus.Ddd.Dtos;

namespace Heus.AspNetCore.Tests.Application;

public class PeopleAppServiceTests: WebIntegratedTestBase<AspNetWebApplicationFactory>
{
    private IPeopleAppService _peopleAppService;
    public PeopleAppServiceTests(AspNetWebApplicationFactory factory) : base(factory)
    {
        _peopleAppService = CreateServiceProxy<IPeopleAppService>();
    }
    protected override async Task BeforeTestAsync()
    {
        await    _peopleAppService.CreateAsync(new() { Name = "test1", Phone = "1310000000" });
        await _peopleAppService.CreateAsync(new() { Name = "test2", Phone = "1320000000" });
    }
    //private readonly IPeopleAppService _peopleAppService;
    //private readonly IRepository<Person> _repository;
    //public PeopleAppServiceTests(AspNetWebApplicationFactory factory)
    //{
    //    _factory = factory;
    //    _repository = repository;
    //    _peopleAppService = _factory.GetServiceProxy<IPeopleAppService>();
    //}

    [Fact]
    public async Task GetList_Test()
    {
        var search = new DynamicSearch<Person>();
        var result = await _peopleAppService.SearchAsync(search);
        result.Total.ShouldBeGreaterThan(0);
        search.AddEqualFilter(s => s.Name, "test1");
        var result2 = await _peopleAppService.SearchAsync(search);
        result2.Total.ShouldBe(1);
         search.AddEqualFilter(s => s.Name, "test1_notexist");
        var result3 = await _peopleAppService.SearchAsync(search);
        result2.Total.ShouldBe(0);
    }




}