
namespace Heus.Core.Ioc;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Indicates that  a  class  with the attribute   is a "Service".
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ServiceAttribute : Attribute
{
    public ServiceLifetime LifeTime { get; }
    public ServiceAttribute(ServiceLifetime lifeTime = ServiceLifetime.Scoped)
    {
        LifeTime = lifeTime;
    }

}


