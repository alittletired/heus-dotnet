namespace Heus.Core.Http.Client;

public class RemoteServiceConfiguration: Dictionary<string, string>
{
    /// <summary>
    /// Base Url.
    /// </summary>
    public string BaseUrl {
        get => this[nameof(BaseUrl)];
        set => this[nameof(BaseUrl)] = value;
    }
    public RemoteServiceConfiguration(string baseUrl)
    {
        this[nameof(BaseUrl)] = baseUrl;
      
    }
}