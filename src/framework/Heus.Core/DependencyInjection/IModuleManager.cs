namespace Heus.Core.DependencyInjection
{
    public interface IModuleManager
    {
        List<ServiceModuleDescriptor> Modules { get; }
        void ConfigureServices(WebApplicationBuilder hostBuilder);
        void Configure(IApplicationBuilder applicationBuilder);

    }
    public class ModuleCreateOptions
    {
        public List<Type> AdditionalModules { get; } = new();
    }

}
