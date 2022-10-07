namespace Heus.Core.Http.Client;

public class HttpClientOptions
{
    public Dictionary<Type, HttpClientProxyConfig> HttpClientProxies { get;  } = new();

}