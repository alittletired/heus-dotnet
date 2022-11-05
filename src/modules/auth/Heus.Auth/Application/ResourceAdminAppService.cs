
using Heus.Auth.Entities;
using Heus.Core.ObjectMapping;
using Heus.Ddd.Application;

using Heus.Ddd.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Heus.Auth.Application
{
    [MapTo(typeof(Resource))]
    public record ResourceDto(string Code, string Name,string Path)
    {
        public int? Sort { get; init; }
        [AllowNull]
        public List<ActionRightDto> Actions { get; set; } = new();
        [AllowNull]
        public List<ResourceDto> Children { get; set; } = new();
    }

    [MapTo(typeof(ActionRight))]
    public record ActionRightDto(string Name,string Title,long ActionMask) { }
    public interface IResourceAdminAppService : IAdminApplicationService<Resource>
    {
        //Task<List<UserMenuDto>> GetUserMenuAsync(long userId);
        Task<bool> SyncResources(List<ResourceDto> resources);
    }
    internal class ResourceAdminAppService :  AdminApplicationService<Resource>, IResourceAdminAppService
    {
        private readonly IRepository<ActionRight> _actionRightActionRepository;
        public ResourceAdminAppService( IRepository<ActionRight> actionRightActionRepository)
        {
            _actionRightActionRepository = actionRightActionRepository;
        }
        [AllowAnonymous]
        public async Task<bool> SyncResources(List<ResourceDto> resourceDtos)
        {
            if (resourceDtos.Count == 0) return false;
            var resources = await Repository.GetListAsync(s=>true);
            var actionRights= await _actionRightActionRepository.GetListAsync(s => true);

            var insertResources=new List<Resource>();
            var insertActionRights=new List<ActionRight>();

            foreach (var resourceDto in resourceDtos)
            {
                var resource = Mapper.Map<Resource>(resourceDto);
                resource.Id = long.Parse(resource.Code);
                insertResources.Add(resource);
            }
            //var menuIds = menus.Select(s => s.Id);
            //var actions=await _menuActionRepository.GetListAsync(s => menuIds.Contains(s.MenuId));

            await Repository.DeleteManyAsync(resources);
            await _actionRightActionRepository.DeleteManyAsync(actionRights);
        
            // await _menuRepository.InsertManyAsync(resources);
            // var actionDtos = menuDtos.SelectMany(r => r.Actions);
            // await _menuActionRepository.DeleteManyAsync();
            return true;

            //_resourceRepository.InsertAsync
        }
       
   
    }
}
