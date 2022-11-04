using Heus.Auth.Domain;
using Heus.Auth.Entities;
using Heus.Ddd.Application;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Microsoft.AspNetCore.Authorization;


namespace Heus.Auth.Application
{
    public  record UserMenuDto(string MenuCode,string ActionMask){}
    public record MenuActionDto(string Name ,long Mask, string? Url)
    {
    }
    public  class  MenuDto
    {
        public string Code { get; init; } = null!;
        public string Name{ get; init; }= null!;
        public string TreeCode{ get; init; }= null!;
        public int Sort { get; init; }
        public List<MenuActionDto> Actions { get; init; } = new();
    }

    public interface IPermissionAdminAppService : IAdminApplicationService<ActionRight, ActionRight, ActionRight>
    {
        Task<List<UserMenuDto>> GetUserMenuAsync(long userId);
        Task<bool> SyncMenus(List<MenuDto> resources);
    }
    internal class PermissionAdminAppService : IPermissionAdminAppService, IMenuAdminAppService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuAction> _menuActionRepository;
        public MenuAdminAppService( IRepository<Menu> menuRepository,IRepository<MenuAction> menuActionRepository)
        {
            _menuRepository = menuRepository;
            _menuActionRepository = menuActionRepository;
        }
        [AllowAnonymous]
        public async Task<bool> SyncMenus(string? appCode,  List<MenuDto> menuDtos)
        {
            if (menuDtos.Count == 0) return false;
            var menus = await _menuRepository.GetListAsync(s => s.AppCode ==appCode);
            var menuIds = menus.Select(s => s.Id);
            var actions=await _menuActionRepository.GetListAsync(s => menuIds.Contains(s.MenuId));
                
            await _menuRepository.DeleteManyAsync(menus);
            await _menuActionRepository.DeleteManyAsync(actions);
            
            // await _menuRepository.InsertManyAsync(resources);
            // var actionDtos = menuDtos.SelectMany(r => r.Actions);
            // await _menuActionRepository.DeleteManyAsync();
            return true;

            //_resourceRepository.InsertAsync
        }
       
        public Task<Menu> CreateAsync(Menu createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

  

        public Task<PageList<Menu>> SearchAsync(DynamicSearch<Menu> input)
        {
            throw new NotImplementedException();
        }

        public Task<Menu> UpdateAsync(Menu updateDto)
        {
            throw new NotImplementedException();
        }

        Task<Menu> IGetOneAppService<Menu>.GetAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
