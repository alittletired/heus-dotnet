using Heus.Business.Dtos;
using Heus.Business.Entities;
using Heus.Ddd.Application.Services;

namespace Heus.Business.AppServices;

public class UserAppService:ICreateAppService<User,UserCreateDto>
    ,IUpdateAppService<User,User>
    ,IDeleteAppService
{
    public Task<User> CreateAsync(UserCreateDto createDto)
    {
        throw new NotImplementedException();

    }

    public Task DeleteAsync(EntityId id)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User updateDto)
    {
        throw new NotImplementedException();
    }
}