using Heus.Core.Uow;

namespace Heus.Core.DependencyInjection;

public static class UnitOfWorkExtensions
{
    public async static  Task  PerformUowTask(this IServiceProvider serviceProvider,Func<IServiceScope, Task> task)
    {
       using var scope = serviceProvider.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        //集成测试会开启作用域，导致此处的工作单位为childunitofwork无效，故需创建新的作用域
        using var uow= manager.Begin(requiresNew:true);
        await task(scope);
        await uow.CompleteAsync();

    }
}