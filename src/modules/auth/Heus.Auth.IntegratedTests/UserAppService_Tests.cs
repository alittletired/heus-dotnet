using Heus.Auth.Application;
using Heus.Auth.Domain;
using Heus.Auth.Dtos;
using Heus.Ddd.Domain;
using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Heus.Ddd.Uow;
using Heus.TestBase;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth.IntegratedTests;

public class UserAppServiceTests : IntegratedTestBase<AuthTestModule>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUserAdminAppService _userService;
    public const string NotExistName = "test2";
    public const string ExistName = "test1";
    private readonly UserCreateDto _existsDto = new() { PlaintextPassword = "123456", Name = "test1", NickName = "test1", Phone = "18912345678" };
    private readonly UserCreateDto _noExistsDto = new() { PlaintextPassword = "123456", Name = "test2", NickName = "test2", Phone = "18922345678" };
    private User _existsUser = null!;

    public UserAppServiceTests()
    {
        _userService = GetRequiredService<IUserAdminAppService>();
        _userRepository = GetRequiredService<IRepository<User>>();
       
    }

    protected async override Task BeforeTestAsync()
    {
        if (!await _userRepository.ExistsAsync(s => s.Name == _existsDto.Name))
        {
            _existsUser = await _userService.CreateAsync(_existsDto);
        }
    }
    [Fact]
    public async Task Create_Test()
    {
        var dto = _noExistsDto;
        await ServiceProvider.PerformUowTask(() => _userService.CreateAsync(dto));
        var user = await _userRepository.Query.FirstAsync(s => s.Name == dto.Name);
        user.NickName.ShouldBe(dto.NickName);
        user.Phone.ShouldBe(dto.Phone);
        user.Password.ShouldNotBeNull();
        user.Password.ShouldNotBe("123456");
    }
    [Fact]
    public async Task GetAsync_Test()
    {
        var result = await _userService.GetAsync(_existsUser.Id);
        result.ShouldNotBeNull();
    }
    [Fact]
    public void GetAsync_ThrowEntityNotFound()
    {
        Assert.ThrowsAsync<EntityNotFoundException>(() => _userService.GetAsync(300));
    }
    [Fact]
    public async Task GetListAsync_Test()
    {
        var dynamicQuery = new DynamicSearch<User>();
        var result = await _userService.SearchAsync(dynamicQuery);
        result.Total.ShouldBeGreaterThan(0);
        dynamicQuery.AddEqualFilter(s => s.Name, "admin1");
        var result2 = await _userService.SearchAsync(dynamicQuery);
        result2.Total.ShouldBe(0);
    }



    [Theory]
    [InlineData("13712345568")]
    public async Task UpdateAsync_Test(string phone)
    {
        var user = await _userService.GetAsync(_existsUser.Id);

        var updateDto = new UserUpdateDto()
        {
            Id = _existsUser.Id,
            Phone = phone,
            Name = user.Name,
            NickName = user.NickName,

        };
        var updateUser = await _userService.UpdateAsync(updateDto);

        updateUser.ShouldNotBeNull();
        updateUser.Phone.ShouldBe(phone);

    }
    [Fact]

    public async Task DeleteAsync_Test()
    {
        await _userService.DeleteAsync(_existsUser.Id);

    }

   
}