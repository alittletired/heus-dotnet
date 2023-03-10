namespace Heus.AspNetCore.Http;

public interface IProxyHttpClientFactory
{
    HttpClient CreateClient(string name);
}