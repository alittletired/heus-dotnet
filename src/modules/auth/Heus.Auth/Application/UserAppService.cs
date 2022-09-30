using Heus.Enroll.Service.Application.Dtos;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Application;
using Heus.Auth.Entities;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heus.Enroll.Service.Application;

public class UserAppService :ApplicationService<User>
    , ICreateAppService<User, UserCreateDto>
    , IUpdateAppService<User, User>
    , IDeleteAppService
   
{
    public virtual Task<User> CreateAsync(UserCreateDto createDto)
    {
        throw new NotImplementedException();

    }

    public async Task DeleteAsync(EntityId id)
    {
        await Repository.DeleteByIdAsync(id);
    }

    public virtual Task<User> UpdateAsync(User updateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetListAsync(UserDto input)
    {
        var query = await Repository.GetQueryableAsync();
        return  await query.ToListAsync();

    }
}