

using Heus.Core.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.TestBase;
public abstract class TestBaseWithServiceProvider
{
    protected IServiceProvider ServiceProvider { get; set; } = null!;

    protected  T? GetService<T>() where T : notnull
    {
        return ServiceProvider.GetService<T>();
    }

    protected  T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

    protected Task WithUnitOfWorkAsync(Func<Task> func)
    {
        return WithUnitOfWorkAsync(new UnitOfWorkOptions(), func);
    }

    protected async Task WithUnitOfWorkAsync(UnitOfWorkOptions options, Func<Task> action)
    {
        using (var uow = UnitOfWorkManager.Begin(ServiceProvider,options, true))
        {
            await action();
            await uow.CompleteAsync();
        }
    }
}
