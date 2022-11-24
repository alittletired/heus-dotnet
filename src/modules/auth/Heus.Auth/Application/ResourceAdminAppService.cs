
using Heus.Core.ObjectMapping;
using Heus.Ddd.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Heus.Auth.Application;

[MapTo(typeof(Resource))]
public record ResourceDto(string Code, string Name, string Path)
{
    public int? Sort { get; init; }
    public IEnumerable<ResourceDto>? Children { get; set; }
    public IEnumerable<ActionDto>? Actions { get; set; }

}

[MapTo(typeof(ActionRight))]
public record ActionDto(string Name, int Flag, string Title, string? Url)
{
}


public record UserActionRight(string ResourcePath, int Flag)
{

}


public interface IResourceAdminAppService : IAdminApplicationService<Resource>
{

    Task<bool> SyncResources(List<ResourceDto> resources);

    Task<IEnumerable<UserActionRight>> GetUserActionRights(long userId);
}

internal class ResourceAdminAppService : AdminApplicationService<Resource>, IResourceAdminAppService
{
    private readonly IRepository<ActionRight> _actionRightRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RoleActionRight> _roleActionRightRepository;
    private readonly IRepository<Resource> _resourceRepository;
    
    public ResourceAdminAppService(IRepository<ActionRight> actionRightRepository
        , IRepository<User> userRepository
        , IRepository<RoleActionRight> roleActionRightRepository
        , IRepository<Resource> resourceRepository,
        IRepository<UserRole> userRoleRepository)
    {
        _actionRightRepository = actionRightRepository;
        _userRepository = userRepository;
        _roleActionRightRepository = roleActionRightRepository;
        _resourceRepository = resourceRepository;
        _userRoleRepository = userRoleRepository;
    }


    private void ExtractResourceTree(IEnumerable<ResourceDto> dtos, List<Resource> resources,
        List<ActionRight> actionRights,Resource? parent)
    {
        foreach (var dto in dtos)
        {
            var resource = Mapper.Map<Resource>(dto);
            resource.Id = long.Parse(resource.Code);
            resources.Add(resource);
            var parentTreeCode = parent == null ? "" : parent.TreeCode + ".";
            resource.TreeCode = parentTreeCode + resource.Code;
            resource.ParentId = parent?.Id;
            if (dto.Children != null)
            {
             resource.Type= ResourceType.MenuGroup;
                ExtractResourceTree(dto.Children, resources, actionRights, resource);
                continue;
            }
            resource.Type= ResourceType.Menu;
            var actions = new List<ActionDto>(dto.Actions ?? new List<ActionDto>());
            //每个资源默认应该添加查询权限
            actions.Insert(0, new ActionDto("view", 1, "查看", null));
            foreach (var action in actions)
            {
                var actionRight = Mapper.Map<ActionRight>(action);
                actionRight.ResourceId = resource.Id;
                //使用pad防止id重复
                actionRight.Id = long.Parse(resource.Code + action.Flag.ToString().PadLeft(10,'0'));

                actionRights.Add(actionRight);
            }
        }
    }

    [AllowAnonymous]
    public async Task<bool> SyncResources(List<ResourceDto> dtos)
    {
        if (dtos.Count == 0)
        {
            return false;
        }
        var insertResources = new List<Resource>();
        var insertActionRights = new List<ActionRight>();
        ExtractResourceTree(dtos, insertResources, insertActionRights,null);
        var deleteCount = await Repository.Query.ExecuteDeleteAsync();
        var deleteActionRightCount = await _actionRightRepository.Query.ExecuteDeleteAsync();
        await Repository.InsertManyAsync(insertResources);
        await _actionRightRepository.InsertManyAsync(insertActionRights);
        return true;
        //_resourceRepository.InsertAsync
    }

    public async Task<IEnumerable<UserActionRight>> GetUserActionRights(long userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user.IsSuperAdmin)
        {
            var query = from ar in _actionRightRepository.Query
                join r in _resourceRepository.Query on ar.ResourceId equals r.Id
                select new UserActionRight(r.Path, ar.Flag);
            var data = await query.ToListAsync();
            return data.GroupBy(s => s.ResourcePath)
                .Select(s => new UserActionRight(s.Key, s.Sum(p => p.Flag)));
        }
        var query1 = from r in _resourceRepository.Query
            join rar in _roleActionRightRepository.Query on r.Id equals rar.ResourceId
            join ur in _userRoleRepository.Query on rar.RoleId equals ur.Id
            where ur.UserId == userId
            select new UserActionRight(r.Path, rar.Flag);
        var data1= await query1.ToListAsync();
        return data1.GroupBy(s => s.ResourcePath)
            .Select(s => new UserActionRight(s.Key, s.Sum(p => p.Flag)));

    }
}
