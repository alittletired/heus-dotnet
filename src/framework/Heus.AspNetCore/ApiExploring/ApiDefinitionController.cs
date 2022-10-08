using Heus.Core.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;


namespace Heus.AspNetCore.ApiExploring
{
    [Route("api/[controller]")]
    public class ApiDefinitionController : Controller, IRemoteService
    {
        private readonly IApiDescriptionGroupCollectionProvider _descriptionProvider;

        public ApiDefinitionController(IApiDescriptionGroupCollectionProvider descriptionProvider)
        {
            _descriptionProvider = descriptionProvider;
        }

        public Task<ApiDescriptionGroupCollection> Index()
        {
            return Task.FromResult(_descriptionProvider.ApiDescriptionGroups);

        }
    }
}
