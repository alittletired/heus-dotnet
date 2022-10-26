using Heus.Auth.Domain;
using Heus.Auth.Entities;
using Heus.Core.Security;
using Heus.Ddd.Application;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Heus.Auth.Application
{
    public interface IResourceAdminAppService: IAdminApplicationService<Resource, Resource, Resource>
    {
       Task< List<string>> getUserPermissions(long userId);
    }
    internal class ResourceAdminAppService : AdminApplicationService, IResourceAdminAppService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RoleResource> _roleResourceRepository;
        private readonly IRepository<Resource> _resourceRepository;

 
        public ResourceAdminAppService(IUserRepository userRepository, IRepository<UserRole> userRoleRepository, IRepository<RoleResource> roleResourceRepository, IRepository<Resource> resourceRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleResourceRepository = roleResourceRepository;
            _resourceRepository = resourceRepository;
        }

        
        public Task<Resource> CreateAsync(Resource createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> getUserPermissions(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user.IsSuperAdmin)
            {
                var query = from a in _resourceRepository.Query
                            join m in _resourceRepository.Query on a.ParentId equals m.Id
                            where a.Type == ResourceType.Menu
                            select m.TreePath + ":" + a.ActionCode;
                return await query.ToListAsync();
            }

            var query1 = from a in _resourceRepository.Query
                         join m in _resourceRepository.Query on a.ParentId equals m.Id
                         join rr in _roleResourceRepository.Query on a.Id equals rr.ResourceId
                         join ur in _userRoleRepository.Query on rr.RoleId equals ur.RoleId
                         join r in _roleResourceRepository.Query on ur.RoleId equals r.Id
                         where a.Type == ResourceType.Menu
                         select m.TreePath + ":" + a.ActionCode;
            return await query1.ToListAsync();
        }

        public Task<PagedList<Resource>> QueryAsync(DynamicQuery<Resource> input)
        {
            throw new NotImplementedException();
        }

        public Task<Resource> UpdateAsync(Resource updateDto)
        {
            throw new NotImplementedException();
        }

        Task<Resource> IGetOneAppService<Resource>.GetAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
