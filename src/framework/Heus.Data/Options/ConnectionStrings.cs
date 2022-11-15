

namespace Heus.Data.Options;

public class ConnectionStrings : Dictionary<string, string>
{

    public const string DefaultConnectionStringName = "Default";

    public string Default
    {
        get => this[DefaultConnectionStringName];
        set => this[DefaultConnectionStringName] = value;
    }
}

