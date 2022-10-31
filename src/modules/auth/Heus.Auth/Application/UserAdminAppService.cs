using Heus.Auth.Domain;
using Heus.Auth.Dtos;
using Heus.Ddd.Application;
using Heus.Auth.Entities;
using Heus.Core.Security;
using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth.Application;
public interface IUserAdminAppService:IAdminApplicationService<UserCreateDto,UserUpdateDto,UserDto>
{
    Task<IEnumerable<long>>  GetUserRoleIdsAsync(long id);
    Task<List<string>> GetResourceCodes(long userId);
}

internal class UserAdminAppService : AdminApplicationService, IUserAdminAppService,IUserService
{
    private readonly IRepository<Organ> _organRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RoleResource> _roleResourceRepository;
    private readonly IRepository<Resource> _resourceRepository;

    public UserAdminAppService(IRepository<Organ> organRepository, IUserRepository userRepository,
        IRepository<UserRole> userRoleRepository, IRepository<RoleResource> roleResourceRepository, IRepository<Resource> resourceRepository)
    {
        _organRepository = organRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _roleResourceRepository = roleResourceRepository;
        _resourceRepository = resourceRepository;
    }

    public async Task<UserDto> GetAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return Mapper.Map<UserDto>(user);
    }

    public async Task<PageList<UserDto>> QueryAsync(DynamicQuery<UserDto> input)
    {
        var query = from u in _userRepository.Query
            select  u;
        var data1 = await query.ToPageListAsync(input);
        return data1;
        // var query1 = from u in _userRepository.GetQueryable()
        //      from b in _userRoleRepository.GetQueryable()
        //        where b.UserId==u.Id
        //  
        //     select  new {u,b};
        // // var b= query..ProjectToType<UserDto>()
        // var data = await query1.ToPageListAsync(input);
        // return data;
    }



    public virtual Task<UserDto> CreateAsync(UserCreateDto createDto)
    {
        throw new NotImplementedException();

    }

    public async Task DeleteAsync(long id)
    {
        await _userRepository.DeleteByIdAsync(id);
    }

    public async Task<UserDto> UpdateAsync(UserUpdateDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(updateDto.Id);
        return Mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<long>> GetUserRoleIdsAsync(long id)
    {
        return await _userRoleRepository.Query.Where(s => s.UserId == id).Select(s => s.RoleId)
            .ToListAsync();
    }

    public async Task<bool> ResetPasswordAsync(RestPasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        //user.SetPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<ICurrentUser?> FindByUserNameAsync(string name)
    {
      var user=  await  _userRepository.FindByUserNameAsync(name);
        if (user == null)
        {
            return null;
        }
      return user.MapTo<ICurrentUser>();
    }

    public async Task<List<string>> GetResourceCodes(long userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user.IsSuperAdmin)
        {
            var query = from a in _resourceRepository.Query
                        where a.Type == ResourceType.Menu
                        select a.Code;
            return await query.ToListAsync();
        }

        var query1 = from a in _resourceRepository.Query

                     join rr in _roleResourceRepository.Query on a.Id equals rr.ResourceId
                     join ur in _userRoleRepository.Query on rr.RoleId equals ur.RoleId
                     join r in _roleResourceRepository.Query on ur.RoleId equals r.Id
                     where a.Type == ResourceType.Menu
                     select a.Code;
        return await query1.ToListAsync();
    }
}