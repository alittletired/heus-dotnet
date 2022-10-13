using Heus.Core.Http;
using Heus.Core.Security;
using Heus.Core.Uow;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;

namespace Heus.IntegratedTests;


internal class TestRemoteServiceProxyContributor:IRemoteServiceProxyContributor
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserService _userService;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    public TestRemoteServiceProxyContributor(ITokenProvider tokenProvider, IUserService userService, IUnitOfWorkManager unitOfWorkManager)
    {
        _tokenProvider = tokenProvider;
        _userService = userService;
        _unitOfWorkManager = unitOfWorkManager;
    }

    private string _token="";
    public async Task PopulateRequestHeaders(HttpRequestMessage request)
    {
        if (_token.IsNullOrEmpty())
        {
         
            var options = new UnitOfWorkOptions() { IsTransactional = true };
            using var unitOfWork = _unitOfWorkManager.Begin(options);
            var user =await _userService.FindByUserNameAsync("admin");
            var principal = _tokenProvider.CreatePrincipal(user!, TokenType.Admin);
            _token = _tokenProvider.CreateToken(principal);
        }

        if (!request.Headers.Contains("Authorization"))
        {
            request.Headers.Add("Authorization","Bearer "+_token);
        }
      
      
    }
}