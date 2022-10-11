using Heus.Auth.Domain;
using Heus.Auth.Dtos;
using Heus.Ddd.Application;
using Heus.Auth.Entities;
using Heus.Core.Security;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Heus.Ddd.Extensions;

namespace Heus.Auth.Application;
public interface IUserAdminAppService:IAdminApplicationService<UserCreateDto,UserUpdateDto,UserDto>
{
    Task<IEnumerable<EntityId>>  GetUserRoleIds(EntityId id);
}

internal class UserAdminAppService : AdminApplicationService, IUserAdminAppService,IUserService
{
    private readonly IRepository<Organ> _organRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;

    public UserAdminAppService(IRepository<Organ> organRepository, IUserRepository userRepository,
        IRepository<UserRole> userRoleRepository)
    {
        _organRepository = organRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<UserDto> GetAsync(EntityId id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return Mapper.Map<UserDto>(user);
    }

    public async Task<PagedList<UserDto>> GetListAsync(DynamicQuery<UserDto> input)
    {
        var query = from u in _userRepository.GetQueryable()
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

    public async Task DeleteAsync(EntityId id)
    {
        await _userRepository.DeleteByIdAsync(id);
    }

    public async Task<UserDto> UpdateAsync(EntityId id, UserUpdateDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(updateDto.Id);
        return Mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<EntityId>> GetUserRoleIds(EntityId id)
    {
        return await _userRoleRepository.GetQueryable().Where(s => s.UserId == id).Select(s => s.RoleId)
            .ToListAsync();
    }

    public async Task<bool> ResetPassword(RestPasswordDto dto)
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
      return user;
    }
}