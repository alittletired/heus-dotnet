using Heus.Core.DependencyInjection;

namespace Heus.Ddd.Data.Filtering;

public interface IDataFilter<TFilter>: ISingletonDependency
    where TFilter : class
{
    IDisposable Enable();

    IDisposable Disable();

    bool IsEnabled { get; }
}

public interface IDataFilter
{
    IDisposable Enable<TFilter>()
        where TFilter : class;

    IDisposable Disable<TFilter>()
        where TFilter : class;

    bool IsEnabled<TFilter>()
        where TFilter : class;
}
