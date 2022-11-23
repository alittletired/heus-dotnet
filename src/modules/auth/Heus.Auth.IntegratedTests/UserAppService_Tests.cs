using Heus.Auth.Application;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Ddd.Domain;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Heus.IntegratedTests;


namespace Heus.Auth.IntegratedTests;
[UseUnitOfWork]
[Collection(nameof(IntegratedTestCollection))]
[TestCaseOrderer("Heus.IntegratedTests.PriorityOrderer", "Heus.IntegratedTests")]
public class UserAppServiceTests: IAsyncLifetime
{
    IntegratedTest<Program> _factory;
    private readonly IUserAdminAppService _userService;
    public const string NotExistName = "test2";
    public const string ExistName = "test1";
    private long _existId = 300;
    public UserAppServiceTests(IntegratedTest<Program> factory)
    {
        _factory = factory;
        _userService = _factory.GetServiceProxy<IUserAdminAppService>(AuthConstants.ServiceName);

    }

    public async  Task InitializeAsync()
    {
        using var uow = UnitOfWorkManagerAccessor.Begin();
        var user = await _userService.CreateAsync(new UserCreateDto { InitialPassword = "123456", Name = ExistName, NickName = ExistName, Phone = "123456" });
        _existId = user.Id;
       await uow.CompleteAsync();
    }

    public async Task DisposeAsync()
    {
         await _userService.DeleteAsync(_existId);
    }


    protected IRepository<User> _userRepository => _factory.Services.GetRequiredService<IRepository<User>>();
    [Fact, TestPriority(-5)]
    public  Task Create() {
        //var user = await _userService.CreateAsync(new UserCreateDto { InitialPassword = "123456", Name = ExistName, NickName = ExistName, Phone = "123456" });
        return Task.CompletedTask;
    }
    [Fact]
    public async Task  GetAsync( )
    {
       var result= await _userService.GetAsync(_existId) ;
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
        dynamicQuery.Filters[nameof(User.Name)] =new DynamicSearchFilter("eq","admin1",null) ;
        var result2 = await _userService.SearchAsync(dynamicQuery);
        result2.Total.ShouldBe(0);
    }
  
    
    
    [Theory]
    [InlineData("13712345568")]
    public async Task UpdateAsync(string phone)
    {
      var user=  await _userService.GetAsync(_existId);
        user.Phone.ShouldNotBe(phone);
        var updateDto = new UserUpdateDto()
        {
            Id =_existId ,
            Phone = phone,Name=user.Name,
            NickName=user.NickName,

        };
     var updateUser  = await _userService.UpdateAsync(updateDto);

        updateUser.ShouldNotBeNull();
        updateUser.Phone.ShouldBe(phone);
      
    }
    [Fact]
    
    public async Task DeleteAsync()
    {
        await _userService.DeleteAsync(_existId);
    
    }

 

 
}