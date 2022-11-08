using Heus.Auth.Domain;
using Heus.Auth.Dtos;
using Heus.Ddd.Application;
using Heus.Auth.Entities;
using Heus.Core.Security;
using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;

namespace Heus.Auth.Application;
public interface IUserAdminAppService:IAdminApplicationService<User, UserDto, User,User>
{
    
    
}

internal class UserAdminAppService : AdminApplicationService<User, UserDto, User, User>, IUserAdminAppService,IUserService
{
    private readonly IRepository<UserRole>  _userRoleResourceRepository;
    public UserAdminAppService(IRepository<UserRole> userRoleResourceRepository)
    {
        _userRoleResourceRepository = userRoleResourceRepository;

    }
    //public async Task<bool> ResetPasswordAsync(RestPasswordDto dto)
    //{
    //    var user = await _userRepository.GetByIdAsync(dto.UserId);
    //    //user.SetPassword(dto.NewPassword);
    //    await _userRepository.UpdateAsync(user);
    //    return true;
    //}
    public override Task<PageList<UserDto>> SearchAsync(DynamicSearch<UserDto> input)
    {
        var query1 = from u in Repository.Query
                     join ur in _userRoleResourceRepository.Query 
                     on u.Id equals ur.UserId into UserRoles
                     from ur1 in UserRoles.DefaultIfEmpty()
                     select new { u, ur1,name=u.Name,bb=ur1.UserId };

        //var data = query1.ToList();
        return query1.ToPageListAsync(input);
    }
    public async Task<ICurrentUser?> FindByNameAsync(string name)
    {
      var user=  await  Repository.FindAsync(u=>u.Name== name);
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