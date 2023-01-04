

namespace Heus.Data.Options;

public class DbConnectionOptions
{
    public Dictionary<string, string> ConnectionStrings { get; set; } = new (StringComparer.OrdinalIgnoreCase);
}
