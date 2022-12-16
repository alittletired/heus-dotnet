using Heus.Auth.Application;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Core.Uow;
using Heus.Ddd.Domain;
using Heus.Ddd.Dtos;

using Heus.Ddd.Repositories;
using Heus.TestBase;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth.IntegratedTests;
public class UserAppServiceTests : IntegratedTestBase<AuthTestModule>
{
    protected readonly IRepository<User> _userRepository;
    private readonly IUserAdminAppService _userService;
    public const string NotExistName = "test2";
    public const string ExistName = "test1";
    private UserCreateDto existsDto = new() { PlaintextPassword = "123456", Name = "test1", NickName = "测试用户1", Phone = "18912345678" };
    private UserCreateDto noExistsDto = new() { PlaintextPassword = "123456", Name = "test2", NickName = "测试用户2", Phone = "18922345678" };
    public UserAppServiceTests()
    {
        _userService = GetRequiredService<IUserAdminAppService>();
        _userRepository = GetRequiredService<IRepository<User>>();
    }
    protected override Task BeforeTest()
    {
        return base.BeforeTest();
    }
    [Fact]

    public async Task Create()
    {
        var dto = noExistsDto;
        await WithUnitOfWorkAsync(() => _userService.CreateAsync(dto));
        var user = await _userRepository.Query.FirstAsync(s => s.Name == dto.Name);
        user.NickName.ShouldBe(dto.NickName);
        user.Phone.ShouldBe(dto.Phone);
        user.Password.ShouldNotBeNull();
        user.Password.ShouldNotBe("123456");
    }
    [Fact]
    public async Task GetAsync()
    {
        var result = await _userService.GetAsync(_existId);
        result.ShouldNotBeNull();
    }
    [Fact]
    public void GetAsync_ThrowEntityNotFound()
    {
        Assert.ThrowsAsync<EntityNotFoundException>(() => _userService.GetAsync(300));
    }
    [Fact]
    public async Task GetListAsync()
    {
        var dynamicQuery = new DynamicSearch<User>();
        var result = await _userService.SearchAsync(dynamicQuery);
        result.Total.ShouldBeGreaterThan(0);
        dynamicQuery.Filters[nameof(User.Name)] = new DynamicSearchFilter("eq", "admin1", null);
        var result2 = await _userService.SearchAsync(dynamicQuery);
        result2.Total.ShouldBe(0);
    }



    [Theory]
    [InlineData("13712345568")]
    public async Task UpdateAsync(string phone)
    {
        var user = await _userService.GetAsync(_existId);
        
        var updateDto = new UserUpdateDto()
        {
            Id = _existId,
            Phone = phone,
            Name = user.Name,
            NickName = user.NickName,

        };
        var updateUser = await _userService.UpdateAsync(updateDto);

        updateUser.ShouldNotBeNull();
        updateUser.Phone.ShouldBe(phone);

    }
    [Fact]

    public async Task DeleteAsync()
    {
        await _userService.DeleteAsync(_existId);

    }

}