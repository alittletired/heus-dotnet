using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Ddd.Application;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;

namespace Heus.Auth.Application;
public interface IRoleAdminAppService:IAdminApplicationService<Role>
{
   
    Task<bool> AuthorizeActionRightsAsync(long id, IEnumerable<ActionRight> actionRights);

}
internal class RoleAdminAppService : AdminApplicationService<Role>, IRoleAdminAppService
{
    private readonly IRepository<RoleActionRight> _roleActionRightRepository;


    public RoleAdminAppService(IRepository<RoleActionRight> roleActionRightRepository)
    {
        _roleActionRightRepository = roleActionRightRepository;
    }

    public async Task<bool> AuthorizeActionRightsAsync(long id, IEnumerable<ActionRight> actionRights)
    {
        var existsRoleActions = await _roleActionRightRepository.GetListAsync(r => r.RoleId == id);
        //var roleActionRights = from ar in actionRights
        //                       group ar by ar.ResourceId into grp
        //                       select new RoleActionRight { ResourceId = grp.Key, RoleId = id, ActionMask = grp.Sum(t => t.ActionMask) };
        //await _roleActionRightRepository.DeleteManyAsync(existsRoleActions);
        //await _roleActionRightRepository.InsertManyAsync(roleActionRights);
        return true;
    }
}