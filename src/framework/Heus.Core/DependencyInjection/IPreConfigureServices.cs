namespace Heus.Core.DependencyInjection;

public interface IPreConfigureServices
{
    void PreConfigureServices(ServiceConfigurationContext context);
}
