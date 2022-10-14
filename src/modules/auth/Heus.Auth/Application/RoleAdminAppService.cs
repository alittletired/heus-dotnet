using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Ddd.Application;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;

namespace Heus.Auth.Application;
public interface IRoleAdminAppService:IAdminApplicationService<RoleCreateDto,RoleUpdateDto,RoleDto>
{
    Task<IEnumerable<long>> GetResourceIdsAsync(long id);
    Task<bool> AuthorizeResourcesAsync(long id, IEnumerable<long> resourceIds);

}
internal class RoleAdminAppService : AdminApplicationService, IRoleAdminAppService
{
    private readonly IRepository<RoleResource> _roleResourceRepository;

    public RoleAdminAppService(IRepository<RoleResource> roleResourceRepository)
    {
        _roleResourceRepository = roleResourceRepository;
    }

    public Task<RoleDto> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<RoleDto>> QueryAsync(DynamicQuery<RoleDto> input)
    {
        throw new NotImplementedException();
    }

    public Task<RoleDto> CreateAsync(RoleCreateDto createDto)
    {
        throw new NotImplementedException();
    }

    public Task<RoleDto> UpdateAsync(RoleUpdateDto updateDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<long>> GetResourceIdsAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AuthorizeResourcesAsync(long id, IEnumerable<long> resourceIds)
    {
       var roleResources=await _roleResourceRepository.GetListAsync(r => r.RoleId == id);
       var deleteItems = roleResources.Where(s => !resourceIds.Contains(s.ResourceId));
       var existsIds = roleResources.Select(s => s.ResourceId).ToList();
       var insertItems = resourceIds.Where(r => !existsIds.Contains(r)).Select(r => new RoleResource()
       {
           ResourceId = r, RoleId = id
       });
       await _roleResourceRepository.InsertManyAsync(insertItems);
       await _roleResourceRepository.DeleteManyAsync(deleteItems);
       return true;
    }
}