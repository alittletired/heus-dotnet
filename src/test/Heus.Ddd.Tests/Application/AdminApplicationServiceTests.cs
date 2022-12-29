using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule.Application;
using Heus.Ddd.TestModule.Domain;

namespace Heus.Ddd.Tests.Application;
public class AdminApplicationServiceTests: DddIntegratedTest
{
    private readonly IUserAppService _userAppService;
    private readonly IRepository<User> _userRepository;
    public AdminApplicationServiceTests() {
        _userAppService=GetRequiredService<IUserAppService>();
        _userRepository = GetRequiredService<IRepository<User>>();
    }

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
