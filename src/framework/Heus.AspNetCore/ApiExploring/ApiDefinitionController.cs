using Heus.Core.Http;
using Heus.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;


namespace Heus.AspNetCore.ApiExploring;

[Route("api/[controller]")]
public class ApiDefinitionController : Controller, IRemoteService
{
    private readonly IApiDescriptionGroupCollectionProvider _descriptionProvider;

    public ApiDefinitionController(IApiDescriptionGroupCollectionProvider descriptionProvider)
    {
        _descriptionProvider = descriptionProvider;
    }

    [HttpGet]
    public Task<string> Index()
    {
        var d = JsonUtils.Serialize(_descriptionProvider.ApiDescriptionGroups);
        return Task.FromResult(d);

    }
}
