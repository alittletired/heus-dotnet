using Microsoft.Extensions.Configuration;

namespace Heus.Core.DependencyInjection;

public interface IModuleManager
{
    List<ServiceModuleDescriptor> Modules { get; }
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    Task InitializeModulesAsync(IServiceProvider serviceProvider);

}
public static class ModuleCreateOptions
{
    public static List<Type> AdditionalModules { get; } = new();
}
