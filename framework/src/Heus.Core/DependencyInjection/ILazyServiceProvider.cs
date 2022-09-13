namespace Heus.Core.DependencyInjection;

public interface ILazyServiceProvider
{
    T LazyGetRequiredService<T>() where T:notnull;
}