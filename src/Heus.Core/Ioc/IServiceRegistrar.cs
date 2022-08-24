

using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ioc;
public interface IServiceRegistrar
{

    void Handle(IServiceCollection services, Type type);
}