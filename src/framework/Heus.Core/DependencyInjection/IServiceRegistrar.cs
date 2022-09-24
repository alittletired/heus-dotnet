using Microsoft.Extensions.DependencyInjection;
namespace Heus.Core.DependencyInjection;
public interface IServiceRegistrar
{

    void Handle(IServiceCollection services, Type type);
}