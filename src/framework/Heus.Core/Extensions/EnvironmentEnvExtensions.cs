using Microsoft.Extensions.Hosting;

namespace Heus.Core.Extensions;

public static class EnvironmentEnvExtensions
{
    public const string Testing = nameof(Testing);
    public static bool IsTesting(this IHostEnvironment hostEnvironment)
    {
      
        return hostEnvironment.IsEnvironment(Testing);
    }
}