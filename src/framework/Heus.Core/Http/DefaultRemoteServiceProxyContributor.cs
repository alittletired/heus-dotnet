using Heus.Core.DependencyInjection;
using Heus.Core.Security;
namespace Heus.Core.Http;

internal class DefaultRemoteServiceProxyContributor : IRemoteServiceProxyContributor, ISingletonDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly ITokenProvider _tokenProvider;

    public DefaultRemoteServiceProxyContributor(ICurrentUser currentUser,
        ITokenProvider tokenProvider)
    {
        _currentUser = currentUser;
        _tokenProvider = tokenProvider;
    }

    public Task PopulateRequestHeaders(HttpRequestMessage request)
    {
        if (!request.Headers.Contains("Authorization") && _currentUser.IsAuthenticated)
        {
            var token = _tokenProvider.CreateToken(_currentUser.Principal!);
            request.Headers.Add("Authorization", "Bearer " + token);
        }

        return Task.CompletedTask;
    }
}
