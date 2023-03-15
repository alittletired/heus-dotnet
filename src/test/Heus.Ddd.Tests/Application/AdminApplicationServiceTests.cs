using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule.Application;
using Heus.Ddd.TestModule.Domain;
using Heus.Ddd.Uow;

namespace Heus.Ddd.Tests.Application;
public class AdminApplicationServiceTests: DddIntegratedTest
{
    private readonly IUserAdminAppService _userAppService;
    private readonly IRepository<User> _userRepository;
    private User _testUser=null!;
    public AdminApplicationServiceTests() {
        _userAppService=GetRequiredService<IUserAdminAppService>();
        _userRepository = GetRequiredService<IRepository<User>>();
    }

    protected async override Task BeforeTestAsync()
    {
        _testUser = await _userAppService.CreateAsync(new() { Name = "TestUserName1", Phone = "1010000000" });

    }

    [Fact]
    public async Task UpdateTest()
    {
        var oldName = string.Empty;
        await ServiceProvider.PerformUowTask(async () =>
        {
            var user = await _userAppService.GetAsync(_testUser.Id);
            oldName = user.Name;
            user.Name ="update"+ oldName;
            await _userAppService.UpdateAsync(user);

        });
        var user = await _userRepository.GetByIdAsync(_testUser.Id);
        user.Name.ShouldBe("update" + oldName);

    }

    [Fact]
    public async Task Get_Test()
    {
        var user =await _userAppService.GetAsync(1);
        user.ShouldNotBeNull();
    }

    [Fact]
    public async Task Search_Test()
    {
        var search = new DynamicSearch<User>();
        var result = await _userAppService.SearchAsync(search);
        result.Total.ShouldBeGreaterThan(0);
       
        search.AddEqualFilter(s => s.Phone, "notexist");
        var result3 = await _userAppService.SearchAsync(search);
        result3.Total.ShouldBe(0);
    }
    
    
}
