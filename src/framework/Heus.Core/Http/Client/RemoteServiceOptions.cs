namespace Heus.Core.Http.Client;

public class RemoteServiceOptions
{
    public  Dictionary<string, RemoteServiceConfiguration> RemoteServices { get;  } = new();
}