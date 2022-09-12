using Microsoft.Extensions.DependencyInjection;
namespace Heus.Core.Ioc;
public interface IServiceRegistrar
{

    void Handle(IServiceCollection services, Type type);
}