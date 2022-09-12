using Microsoft.Extensions.DependencyInjection;


namespace Heus.Core.Ioc;

public interface IPreConfigureServices
{
    void PreConfigureServices(ServiceConfigurationContext context);
}
