using Microsoft.EntityFrameworkCore;
using  System.Collections.Concurrent;
namespace Heus.Data.MySql;

public static class ServerVersionCache
{
    private static readonly ConcurrentDictionary<string, ServerVersion> _cache = new();
    public static ServerVersion GetServerVersion(string connectionString)
    {
       return _cache.GetOrAdd(connectionString, ServerVersion.AutoDetect);
    }
}