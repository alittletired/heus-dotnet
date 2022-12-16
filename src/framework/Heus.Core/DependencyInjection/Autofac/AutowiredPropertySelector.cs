using System.Collections.Generic;
using System.Reflection;
using Autofac.Core;
using Autofac;
using Heus.Core.Utils;
using Autofac.Core.Resolving.Pipeline;
using Mapster;

namespace Heus.Core.DependencyInjection.Autofac;

internal class AutowiredPropertySelector
{
    private readonly Type _type;
    private Dictionary<PropertyInfo, Action<object, object?>> AutowiredProperties { get; } = new();
    public AutowiredPropertySelector(Type type)
    {
        _type = type;

        var properties = type.GetTypeInfo().GetRuntimeProperties()
            .Where(p => p.GetCustomAttribute(typeof(AutowiredAttribute), true) != null);
        foreach (var prop in properties)
        {
            if (!prop.CanWrite)
            {
                continue;
            }
            AutowiredProperties[prop] = ReflectionUtils.MakeFastPropertySetter(prop);
        }
    }
    public void Autowired(ResolveRequestContext context )
    {
        var instance = context.Instance!;
        foreach (var (key, setter) in AutowiredProperties)
        {

            var propertyValue = context.Resolve(key.PropertyType);
            setter(instance, propertyValue);
        }
    }

}
