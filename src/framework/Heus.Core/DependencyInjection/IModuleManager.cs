namespace Heus.Core.DependencyInjection
{
    public interface IModuleManager
    {
        List<ServiceModuleDescriptor> Modules { get; }
        void ConfigureServices(WebApplicationBuilder hostBuilder);
        void Configure(IApplicationBuilder applicationBuilder);

    }
    public static class ModuleCreateOptions
    {
        
        public static List<Type> AdditionalModules { get; } = new();
    }

}
