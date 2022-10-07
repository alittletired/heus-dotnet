namespace Heus.Core.Http.Client;

public class HttpClientProxyConfig
{
    public Type Type { get; }

    public string RemoteServiceName { get; }

    public HttpClientProxyConfig(Type type, string remoteServiceName)
    {
        Type = type;
        RemoteServiceName = remoteServiceName;
    }
}