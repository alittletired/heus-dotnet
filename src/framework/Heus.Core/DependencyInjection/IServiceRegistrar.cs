using Microsoft.Extensions.DependencyInjection;
namespace Heus.Core.DependencyInjection;

public class ServiceRegistrarChain
{
    private readonly List<IServiceRegistrar> _serviceRegistrars;
    private int _current;

    public ServiceRegistrarChain(List<IServiceRegistrar> serviceRegistrars)
    {
        _serviceRegistrars = serviceRegistrars;
        _current = serviceRegistrars.Count;
    }

    public void Next(IServiceCollection services, Type type)
    {
        _current--;
        if (_current < 0)
        {
            return;
        }

        _serviceRegistrars[_current].Handle(services, type, this);
    }
}

public interface IServiceRegistrar
{

    void Handle(IServiceCollection services, Type type,ServiceRegistrarChain chain);
}