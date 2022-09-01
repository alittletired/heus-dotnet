using Heus.Ioc;


namespace Heus.Core.Ioc;

public interface IPreConfigureServices
{
    void PreConfigureServices(ConfigureServicesContext context);
}
