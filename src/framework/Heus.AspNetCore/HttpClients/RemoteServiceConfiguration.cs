namespace Heus.AspNetCore.HttpClients;

public class RemoteServiceConfiguration : Dictionary<string, string>
{
    public RemoteServiceConfiguration(string baseUrl)
    {
        BaseUrl = baseUrl;
    }
    public string BaseUrl
    {
        get => this[nameof(BaseUrl)];
        set => this[nameof(BaseUrl)] = value;
    }
}

    
