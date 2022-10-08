using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.DependencyInjection
{
    public class DependencyAttribute: Attribute
    {
        public  ServiceLifetime? Lifetime { get; set; }
        public virtual bool ReplaceServices { get; set; }
        public virtual bool TryRegister { get; set; }
    }
}
