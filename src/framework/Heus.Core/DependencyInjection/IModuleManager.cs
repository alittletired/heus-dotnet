namespace Heus.Core.DependencyInjection
{
    public interface IModuleManager
    {
        List<ServiceModuleDescriptor> Modules { get; }
        void ConfigureServices(IHostBuilder hostBuilder);
        void Configure(IApplicationBuilder applicationBuilder);

    }
    public class ModuleCreateOptions
    {
        public List<Type> AdditionalModules { get; } = new();
    }

}
