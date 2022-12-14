using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.DependencyInjection;

public class DependencyAttribute: Attribute
{
    public  ServiceLifetime Lifetime { get; set; }
    public  bool ReplaceServices { get; set; }
    public  bool TryRegister { get; set; }
}
