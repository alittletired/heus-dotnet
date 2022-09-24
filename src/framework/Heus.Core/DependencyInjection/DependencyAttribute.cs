using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.DependencyInjection
{
    public class DependencyAttribute: Attribute
    {
        public  ServiceLifetime? Lifetime { get; set; }
    }
}
