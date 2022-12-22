using Heus.Core.Uow;

namespace System;

public static class UnitOfWorkExtensions
{
    public async static  Task  PerformUowTask(this IServiceProvider serviceProvider,Func<IServiceProvider,Task> task)
    {
     
        var manager =serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        //集成测试会开启作用域，导致此处的工作单位为childunitofwork无效，故需创建新的作用域
        using var uow= manager.Begin(serviceProvider,requiresNew:true);
        await task(serviceProvider);
        await uow.CompleteAsync();

    }
}