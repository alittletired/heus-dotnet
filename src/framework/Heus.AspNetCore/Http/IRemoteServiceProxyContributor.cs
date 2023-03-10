namespace Heus.AspNetCore.Http;

public interface IRemoteServiceProxyContributor
{
    Task PopulateRequestHeaders(HttpRequestMessage request);
}