using Heus.Enroll.Service.Application.Dtos;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Application;
using Heus.Auth.Entities;

namespace Heus.Enroll.Service.Application;

public class UserAppService : ICreateAppService<User, UserCreateDto>
    , IUpdateAppService<User, User>
    , IDeleteAppService
{
    public virtual Task<User> CreateAsync(UserCreateDto createDto)
    {
        throw new NotImplementedException();

    }

    public virtual Task DeleteAsync(EntityId id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<User> UpdateAsync(User updateDto)
    {
        throw new NotImplementedException();
    }
}