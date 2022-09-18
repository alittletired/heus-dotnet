namespace Heus.Core.Modularity;

public interface IModuleContainer
{
    IReadOnlyList<ServiceModuleDescriptor> Modules { get; }
}