
using Heus.Auth.Entities;
using Heus.Core.ObjectMapping;
using Heus.Ddd.Application;

using Heus.Ddd.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Heus.Auth.Application
{
    [MapTo(typeof(Resource))]
    public record ResourceDto(string Code, string Name, string Path)
    {
        public int? Sort { get; init; }
        public IEnumerable<ActionRightDto>? Actions { get; init; } 
    
    }

    [MapTo(typeof(ActionRight))]
    public record ActionRightDto(string Name, string Display, string Url) { }


    public interface IResourceAdminAppService : IAdminApplicationService<Resource>
    {
        //Task<List<UserMenuDto>> GetUserMenuAsync(long userId);
        //Task<bool> SyncResources(List<ActionRightDto> resources);

        Task<IEnumerable<ResourceDto>> GetUserResources(long userId);
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
        [AllowAnonymous]
        public async Task<bool> SyncResources(List<ResourceDto> resourceDtos)
        {
            if (resourceDtos.Count == 0) return false;
            var resources = await Repository.GetListAsync(s => true);
            //var actionRights = await _actionRightActionRepository.GetListAsync(s => true);

            //var insertResources = new List<Resource>();
            //var insertActionRights = new List<ActionRight>();

            //foreach (var resourceDto in resourceDtos)
            //{
            //    var resource = Mapper.Map<Resource>(resourceDto);
            //    resource.Id = long.Parse(resource.Code);
            //    insertResources.Add(resource);
            //}
            ////var menuIds = menus.Select(s => s.Id);
            ////var actions=await _menuActionRepository.GetListAsync(s => menuIds.Contains(s.MenuId));

            //await Repository.DeleteManyAsync(resources);
            //await _actionRightActionRepository.DeleteManyAsync(actionRights);

            // await _menuRepository.InsertManyAsync(resources);
            // var actionDtos = menuDtos.SelectMany(r => r.Actions);
            // await _menuActionRepository.DeleteManyAsync();
            return true;

            //_resourceRepository.InsertAsync
        }

        public async Task<IEnumerable<ResourceDto>> GetUserResources(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user.IsSuperAdmin)
            {
                var query = from r in _resourceRepository.Query
                            join ar in _actionRightRepository.Query on r.Id equals ar.ResourceId     into grouping
                            select new ResourceDto(r.Code,r.Name, r.Path) { Sort=r.Sort,
                            Actions=grouping.Select(a=> new  ActionRightDto(a.Name,a.Display,a.Url))
                            };
                return await query.ToListAsync();
            }
            var query1 = from r in _resourceRepository.Query
                         join ar in _actionRightRepository.Query on r.Id equals ar.ResourceId into grouping
                         join rar in _roleActionRightRepository.Query on r.Id equals rar.ResourceId
                         join ur in _userRoleRepository.Query on rar.RoleId equals ur.Id
                         where ur.UserId == userId
                         select new ResourceDto(r.Code, r.Name, r.Path)
                         {
                             Sort = r.Sort,
                             Actions = grouping.Select(a => new ActionRightDto(a.Name, a.Display, a.Url))
                         };
            return await query1.ToListAsync();

        }
    }
}
