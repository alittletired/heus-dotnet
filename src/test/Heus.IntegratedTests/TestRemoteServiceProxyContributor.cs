using Heus.Core.DependencyInjection;
using Heus.Core.Http;
using Heus.Core.Security;
using Heus.Core.Uow;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;

namespace Heus.IntegratedTests;


internal class TestRemoteServiceProxyContributor : IRemoteServiceProxyContributor,ISingletonDependency
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserService _userService;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IServiceProvider _serviceProvider;
    public TestRemoteServiceProxyContributor(ITokenProvider tokenProvider, IUserService userService, IUnitOfWorkManager unitOfWorkManager, IServiceProvider serviceProvider)
    {
        _tokenProvider = tokenProvider;
        _userService = userService;
        _unitOfWorkManager = unitOfWorkManager;
        _serviceProvider = serviceProvider;
    }

    private string _token = "";
    public async Task PopulateRequestHeaders(HttpRequestMessage request)
    {
        if (_token.IsNullOrEmpty())
        {

            var options = new UnitOfWorkOptions() {IsTransactional = true };
            using var unitOfWork = _unitOfWorkManager.Begin(options);
            var user = await _userService.FindByNameAsync("admin");
            var principal = _tokenProvider.CreatePrincipal(user!, TokenType.Admin);
            _token = _tokenProvider.CreateToken(principal);
        }

        if (!request.Headers.Contains("Authorization"))
        {
            request.Headers.Add("Authorization", "Bearer " + _token);
        }


    }
}