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
      
    }
    internal class ResourceAdminAppService : AdminApplicationService, IResourceAdminAppService
    {
     
        private readonly IRepository<Resource> _resourceRepository;

 
        public ResourceAdminAppService(IUserRepository userRepository, IRepository<UserRole> userRoleRepository
            , IRepository<RoleResource> roleResourceRepository, IRepository<Resource> resourceRepository)
        {
      
        
           
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
