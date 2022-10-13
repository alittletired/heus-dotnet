using Heus.Auth.Application;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Ddd.Domain;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Heus.IntegratedTests;

namespace Heus.Auth.IntegratedTests;

public class UserAppServiceTests:IClassFixture<IntegratedTest<Program>>

{
    private readonly IntegratedTest<Program> _factory;
    private readonly IUserAdminAppService _userService;
    public  const string NotExistId = "633937b08c4b912a68aa072b";
    public  const string ExistId = "333937b08c4b912a68aa072b";

    public UserAppServiceTests(IntegratedTest<Program> factory )
    {
        _factory = factory;
        _userService = _factory.GetServiceProxy<IUserAdminAppService>(AuthConstants.ServiceName);
    }

    protected IRepository<User> _userRepository => _factory.Services.GetRequiredService<IRepository<User>>();
    [Theory]
    [InlineData(ExistId)]
    public async Task  GetAsync(string id )
    {
       var result= await _userService.GetAsync(EntityId.Parse(id)) ;
       result.ShouldNotBeNull();
    }
    [Theory]
    [InlineData(NotExistId)]
    public  void  GetAsync_ThrowEntityNotFound(string id )
    {
        Assert.ThrowsAsync<EntityNotFoundException>(async ()=>await _userService.GetAsync(EntityId.Parse(id)));
    }
    [Fact]
    public async Task GetListAsync()
    {
        var dynamicQuery = new DynamicQuery<UserDto>();
        var result = await _userService.QueryAsync(dynamicQuery);
        result.Count.ShouldBeGreaterThan(0);
        dynamicQuery.Filters["UserName"] = "admin1";
        var result2 = await _userService.QueryAsync(dynamicQuery);
        result2.Count.ShouldBe(0);
    }

    public Task<UserDto> CreateAsync(UserCreateDto createDto)
    {
        throw new NotImplementedException();
    }
    [Theory]
    [InlineData("13712345568",ExistId)]
    public async Task UpdateAsync(string phone,string id)
    {
       
        User updateDto = new User()
        {
            Id =EntityId.Parse(id) ,
            Phone = phone

        };
        var user = await _userRepository.FindAsync(s=>s.Phone == phone);
        user.ShouldNotBeNull();
        user.Id.ToString().ShouldBe(id);
    }
    [Theory]
    [InlineData(NotExistId)]
    [InlineData(ExistId)]
    public async Task DeleteAsync(string id)
    {
        await _userService.DeleteAsync(EntityId.Parse(id));
    }
}