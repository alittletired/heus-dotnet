
using Heus.Core.Security;
using Heus.Core.Utils;
using Heus.Ddd.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Heus.Auth.Application;
public interface IUserAdminAppService:IAdminApplicationService<User,User, UserCreateDto,UserUpdateDto>
{
    
    
}

internal class UserAdminAppService : AdminApplicationService<User, User, UserCreateDto, UserUpdateDto>, IUserAdminAppService,IUserService
{


    //public async Task<bool> ResetPasswordAsync(RestPasswordDto dto)
    //{
    //    var user = await _userRepository.GetByIdAsync(dto.UserId);
    //    //user.SetPassword(dto.NewPassword);
    //    await _userRepository.UpdateAsync(user);
    //    return true;
    //}
    public async override Task<User> CreateAsync(UserCreateDto createDto)
    {
        var entity = Mapper.Map<User>(createDto);
        entity.SetPassword(createDto.PlaintextPassword);
        await Repository.InsertAsync(entity);
        return entity;
    }
    [NonAction]
    public async Task<ICurrentUser?> FindByNameAsync(string name)
    {
        var user = await Repository.FindOneAsync(u => u.Name == name);
        if (user == null)
        {
            return null;
        }

        return user.MapTo<ICurrentUser>();
    }

    //public async Task<List<UserMenuDto>> GetUserMenuAsync(long userId)
    //{
    //    var user = await _userRepository.GetByIdAsync(userId);
    //    if (user.IsSuperAdmin)
    //    {
    //        var menuActions = from a in _roleActionRepository.Query
    //                    select a;
    //        return await query.ToListAsync();
    //    }

    //    var query1 = from a in _resourceRepository.Query

    //                 join rr in _roleResourceRepository.Query on a.Id equals rr.ResourceId
    //                 join ur in _userRoleRepository.Query on rr.RoleId equals ur.RoleId
    //                 join r in _roleResourceRepository.Query on ur.RoleId equals r.Id
    //                 where a.Type == ResourceType.Menu
    //                 select a.Code;
    //    return await query1.ToListAsync();
    //}
}