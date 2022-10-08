namespace Heus.Core.Http;

public interface IProxyHttpClientFactory
{
    HttpClient CreateClient(string name);
}