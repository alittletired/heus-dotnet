
namespace Heus.Auth.Application;
public interface IRoleAdminAppService:IAdminApplicationService<Role>
{
   
    Task<bool> AuthorizeActionAsync(long id, IEnumerable<long> actionIds);
    Task<IEnumerable<RoleActionRight>> GetActionIdsAsync(long id);

}
internal class RoleAdminAppService : AdminApplicationService<Role>, IRoleAdminAppService
{
    private readonly IRepository<RoleActionRight> _roleActionRightRepository;
    private readonly IRepository<ActionRight> _actionRightRepository;

    public RoleAdminAppService(IRepository<RoleActionRight> roleActionRightRepository,IRepository<ActionRight> actionRightRepository)
    {
        _roleActionRightRepository = roleActionRightRepository;
        _actionRightRepository = actionRightRepository;
    }

    public async Task<IEnumerable<RoleActionRight>> GetActionIdsAsync(long id)
    {
        var existsRoleActions = await _roleActionRightRepository.FindAllAsync(r => r.RoleId == id);
        return existsRoleActions;

    }
    public async  Task<bool> AuthorizeActionAsync(long id, IEnumerable<long> actionIds)
    {
        var existsRoleActions = await _roleActionRightRepository.FindAllAsync(r => r.RoleId == id);
        var actionRights =await _actionRightRepository.Query.Where(ar => actionIds.Contains(ar.Id)).ToListAsync();
        var roleActionRights = actionRights.GroupBy(s => s.ResourceId)
            .Select(grp =>
            new RoleActionRight { ResourceId = grp.Key, RoleId = id, Flag = grp.Sum(t => t.Flag) });         
                              
        await _roleActionRightRepository.DeleteManyAsync(existsRoleActions);
        await _roleActionRightRepository.InsertManyAsync(roleActionRights);
        return true;
    }
}