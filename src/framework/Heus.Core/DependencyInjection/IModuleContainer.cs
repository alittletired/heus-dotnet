namespace Heus.Core.DependencyInjection;

public interface IModuleContainer
{
    IReadOnlyList<ServiceModuleDescriptor> Modules { get; }
}