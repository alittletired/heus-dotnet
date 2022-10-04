using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Ddd.Application;
using Heus.Ddd.Domain;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Heus.Enroll.Service.Application;
using Heus.IntegratedTests;
using Shouldly;
using Xunit;

namespace Heus.Auth.IntegratedTests;

public class UserAppServiceTests:IntegratedTestBase<Program,IUserManagementService>
{
    public  const string NotExistId = "633937b08c4b912a68aa072b";
    public  const string ExistId = "333937b08c4b912a68aa072b";
    protected IRepository<User> _userRepository => Services.GetRequiredService<IRepository<User>>();
    [Theory]
    [InlineData(ExistId)]
    public async Task  GetAsync(string id )
    {
       var result= await _appService.GetAsync(EntityId.Parse(id)) ;
       result.ShouldNotBeNull();
    }
    [Theory]
    [InlineData(NotExistId)]
    public  void  GetAsync_ThrowEntityNotFound(string id )
    {
        Assert.ThrowsAsync<EntityNotFoundException>(async ()=>await _appService.GetAsync(EntityId.Parse(id)));
    }
    public Task<PagedList<UserDto>> GetListAsync(DynamicQuery<UserDto> input)
    {
        throw new NotImplementedException();
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
        var user = await _userRepository.FindAsync(s=>s.Phone==phone);
        user.ShouldNotBeNull();
        user.Id.ToString().ShouldBe(id);
    }
    [Theory]
    [InlineData(NotExistId)]
    [InlineData(ExistId)]
    public async Task DeleteAsync(string id)
    {
        await _appService.DeleteAsync(EntityId.Parse(id));
    }
}