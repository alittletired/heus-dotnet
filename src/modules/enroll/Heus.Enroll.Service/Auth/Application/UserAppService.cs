using Heus.Enroll.Service.Application.Dtos;
using Heus.Enroll.Service.Entities;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Application.Services;

namespace Heus.Enroll.Service.Application;
[Service]
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