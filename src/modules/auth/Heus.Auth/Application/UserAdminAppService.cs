using Heus.Auth.Domain;
using Heus.Auth.Dtos;
using Heus.Ddd.Application;
using Heus.Auth.Entities;
using Heus.Core.Security;

namespace Heus.Auth.Application;
public interface IUserAdminAppService:IAdminApplicationService<User>
{
    
    
}

internal class UserAdminAppService : AdminApplicationService<User>, IUserAdminAppService,IUserService
{
    //public async Task<bool> ResetPasswordAsync(RestPasswordDto dto)
    //{
    //    var user = await _userRepository.GetByIdAsync(dto.UserId);
    //    //user.SetPassword(dto.NewPassword);
    //    await _userRepository.UpdateAsync(user);
    //    return true;
    //}

    public async Task<ICurrentUser?> FindByUserNameAsync(string name)
    {
      var user=  await  Repository.FindAsync(u=>u.UserName== name);
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