using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Uow;

public static class UnitOfWorkExtensions
{
    public async static  Task  PerformUowTask(this IServiceProvider serviceProvider,Func<IServiceProvider,Task> task)
    {
     
        var manager =serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var scope = serviceProvider.CreateScope();
        using var uow= manager.Begin(scope.ServiceProvider,requiresNew:true);
        await task(serviceProvider);
        await uow.CompleteAsync();

    }
    public async static Task PerformUowTask(this IServiceProvider serviceProvider, Func<Task> task)
    {

        var manager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var scope = serviceProvider.CreateScope();
        using var uow = manager.Begin(scope.ServiceProvider, requiresNew: true);
        await task();
        await uow.CompleteAsync();

    }
}