using Heus.Auth.Domain;
using Heus.Auth.Entities;
using Heus.Ddd.Application;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Microsoft.AspNetCore.Authorization;


namespace Heus.Auth.Application
{
    public class ResourceTreeNode: Resource
    {

        public List<ResourceTreeNode> Children { get; set; } = new();
    }
    public interface IResourceAdminAppService: IAdminApplicationService<Resource, Resource, Resource>
    {
        Task<IEnumerable<Resource>> SyncResource(List<Resource> resources);
    }
    internal class ResourceAdminAppService : AdminApplicationService, IResourceAdminAppService
    {
     
        private readonly IRepository<Resource> _resourceRepository;

 
        public ResourceAdminAppService(IUserRepository userRepository, IRepository<UserRole> userRoleRepository
            , IRepository<RoleResource> roleResourceRepository, IRepository<Resource> resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }
        [AllowAnonymous]
        public async Task<IEnumerable<Resource>> SyncResource( List<Resource> resources)
        {
            if (resources.Count == 0) return resources;
            var existsResources = await _resourceRepository.GetListAsync(s => s.AppCode == resources[0].AppCode);
            await _resourceRepository.DeleteManyAsync(existsResources);
            await _resourceRepository.InsertManyAsync(resources);
            return resources;

            //_resourceRepository.InsertAsync
        }
       
        public Task<Resource> CreateAsync(Resource createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

  

        public Task<PageList<Resource>> SearchAsync(DynamicSearch<Resource> input)
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
