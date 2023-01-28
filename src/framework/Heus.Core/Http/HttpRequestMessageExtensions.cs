using Heus.Core.Security;

namespace Heus.Core.Http;

public static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage PopulateAuthorization(this HttpRequestMessage request,ICurrentUser currentUser)
    { if (!request.Headers.Contains("Authorization") && currentUser.IsAuthenticated)
        {
            var token = _tokenProvider.CreateToken(_currentUser.Principal!);
            request.Headers.Add("Authorization", "Bearer " + token);
        }

        return request;
    }
}