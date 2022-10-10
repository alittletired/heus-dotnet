namespace Heus.Core.Http;

public interface IRemoteServiceProxyContributor
{
    Task PopulateRequestHeaders(HttpRequestMessage request);
}