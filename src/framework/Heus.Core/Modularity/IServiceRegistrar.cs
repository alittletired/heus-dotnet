using Microsoft.Extensions.DependencyInjection;
namespace Heus.Core.Modularity;
public interface IServiceRegistrar
{

    void Handle(IServiceCollection services, Type type);
}