using Heus.Business.Dtos;
using Heus.Business.Entities;
using Heus.Core.Ioc;
using Heus.Ddd.Application.Services;

namespace Heus.Business.AppServices;
[Service]
public class UserAppService:ICreateAppService<User,UserCreateDto>
    ,IUpdateAppService<User,User>
    ,IDeleteAppService
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