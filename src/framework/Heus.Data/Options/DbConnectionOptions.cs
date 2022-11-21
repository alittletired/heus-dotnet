

namespace Heus.Data.Options;

internal class DbConnectionOptions
{
    public Dictionary<string, string> ConnectionStrings { get; set; } = new (StringComparer.OrdinalIgnoreCase);
}
