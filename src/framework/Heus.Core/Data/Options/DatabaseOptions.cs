

namespace Heus.Core.Data.Options;

internal class DatabaseOptions:Dictionary<string,DbConnectionInfo>
{
    public DbConnectionInfo GetDefaultConnectionInfo() {
        return this["Default"];
        }
}
