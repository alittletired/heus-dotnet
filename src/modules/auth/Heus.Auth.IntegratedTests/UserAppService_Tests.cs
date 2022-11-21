using Heus.Auth.Application;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Ddd.Domain;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Heus.IntegratedTests;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;

namespace Heus.Auth.IntegratedTests;

public class UserAppServiceTests:IClassFixture<IntegratedTest<Program>>, IAsyncLifetime

{
    private readonly IntegratedTest<Program> _factory;
    private readonly IUserAdminAppService _userService;
    public  const string NotExistName = "test2";
    public  const string ExistName ="test1" ;
    private long _existId;
    public async Task InitializeAsync()
    {
        var user = await _userService.CreateAsync(new UserCreateDto { InitialPassword = "123456", Name = ExistName, NickName = ExistName, Phone = "123456" });
        _existId = user.Id;
    }

    public async Task DisposeAsync()
    {
         await _userService.DeleteAsync(_existId);
    }
    public UserAppServiceTests(IntegratedTest<Program> factory )
    {
        _factory = factory;
        _userService = _factory.GetServiceProxy<IUserAdminAppService>(AuthConstants.ServiceName);

    }

    protected IRepository<User> _userRepository => _factory.Services.GetRequiredService<IRepository<User>>();
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
       
        User updateDto = new User()
        {
            Id =_existId ,
            Phone = phone

        };
        var user = await _userRepository.FindOneAsync(s=>s.Phone == phone);
        user.ShouldNotBeNull();
        user.Id.ShouldBe(_existId);
    }
    [Fact]
    
    public async Task DeleteAsync()
    {
        await _userService.DeleteAsync(_existId);
    
    }

 

 
}