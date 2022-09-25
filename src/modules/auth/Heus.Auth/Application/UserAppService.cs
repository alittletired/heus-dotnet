using Heus.Enroll.Service.Application.Dtos;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Application;
using Heus.Auth.Entities;

namespace Heus.Enroll.Service.Application;

public class UserAppService :ApplicationService<User>
    , ICreateAppService<User, UserCreateDto>
    , IUpdateAppService<User, User>
    , IDeleteAppService
    , IDynamicQueryAppService<UserDto>
{
    public virtual Task<User> CreateAsync(UserCreateDto createDto)
    {
        throw new NotImplementedException();

    }

    public async Task DeleteAsync(EntityId id)
    {
        await Repository.DeleteAsync(id);
    }

    public virtual Task<User> UpdateAsync(User updateDto)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResultDto<UserDto>> GetListAsync(UserDto input)
    {
        throw new NotImplementedException();
    }
}