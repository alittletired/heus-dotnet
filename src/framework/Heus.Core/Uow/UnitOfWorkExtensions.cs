using Heus.Core.Uow;

namespace Heus.Core.DependencyInjection;

public static class UnitOfWorkExtensions
{
    public async static  Task  PerformUowTask(this IServiceProvider serviceProvider,Func<IServiceScope, Task> task)
    {
       using var scope = serviceProvider.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow= manager.Begin(new() { ServiceProvider = scope.ServiceProvider });
        await task(scope);
        await uow.CompleteAsync();

    }
}