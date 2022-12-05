using Heus.Core.Uow;

namespace Heus.Core.DependencyInjection;

public static class UnitOfWorkExtensions
{
    public async static  Task  PerformUowTask(this IServiceProvider serviceProvider,Func< Task> task)
    {
        var manager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow= manager.Begin();
        await task();
        await uow.CompleteAsync();

    }
}