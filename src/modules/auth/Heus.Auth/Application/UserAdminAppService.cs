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

    public Task<PagedList<UserDto>> GetListAsync(DynamicQuery<UserDto> input)
    {
        var query = from u in _userRepository.GetQueryable()
            from o in _organRepository.GetQueryable()
        
            select u;
        // var b= query..ProjectToType<UserDto>()
        var data = query.ToList();
        throw new NotImplementedException();
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

    public async Task<ICurrentUser?> FindByNameAsync(string name)
    {
      var user=  await  _userRepository.FindByAccountAsync(name);
        if (user == null)
        {
            return null;
        }
      return user;
    }
}