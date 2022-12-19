using Heus.Core.Uow;
using Heus.TestBase;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Tests.Uow;
[TestClass]
public class UnitOfWork_Events_Tests : IntegratedTestBase<CoreModuleInitializer>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    protected override bool AutoCreateUow => false;

    public UnitOfWork_Events_Tests()
    {

        _unitOfWorkManager = RootServiceProvider.GetRequiredService<IUnitOfWorkManager>(); ;
    }
    [TestMethod]
    public async Task Should_Trigger_Complete_On_Success()
    {
        var completed = false;
        var disposed = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.OnCompleted(() =>
            {
                completed = true;
                return Task.CompletedTask;
            });

            uow.Disposed += (sender, args) => disposed = true;

            await uow.CompleteAsync();

            
        }
        completed.ShouldBeTrue();
        disposed.ShouldBeTrue();
    }
    [TestMethod]
    public async Task Should_Trigger_Complete_On_Success_In_Child_Uow()
    {
        var completed = false;
        var disposed = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            using (var childUow = _unitOfWorkManager.Begin())
            {
                childUow.OnCompleted(() =>
                {
                    completed = true;
                    return Task.CompletedTask;
                });

                uow.Disposed += (sender, args) => disposed = true;

                await childUow.CompleteAsync();

                completed.ShouldBeFalse(); //Parent has not been completed yet!
                disposed.ShouldBeFalse();
            }

            completed.ShouldBeFalse(); //Parent has not been completed yet!
            disposed.ShouldBeFalse();

            await uow.CompleteAsync();

            completed.ShouldBeTrue(); //It's completed now!
            disposed.ShouldBeFalse(); //But not disposed yet!
        }

        disposed.ShouldBeTrue();
    }

    [TestMethod]
    public void Should_Not_Trigger_Complete_If_Uow_Is_Not_Completed()
    {
        var completed = false;
        var failed = false;
        var disposed = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.OnCompleted(() =>
            {
                completed = true;
                return Task.CompletedTask;
            });

            uow.Failed += (_, _) => failed = true;
            uow.Disposed += (_, _) => disposed = true;
        }

        completed.ShouldBeFalse();
        failed.ShouldBeTrue();
        disposed.ShouldBeTrue();
    }

    [TestMethod]
    public void Should_Trigger_Failed_If_Uow_Throws_Exception()
    {
        var completed = false;
        var failed = false;
        var disposed = false;

        Assert.ThrowsException<Exception>(new Action(() =>
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                uow.OnCompleted(() =>
                {
                    completed = true;
                    return Task.CompletedTask;
                });

                uow.Failed += (sender, args) => failed = true;
                uow.Disposed += (sender, args) => disposed = true;

                throw new Exception("test exception");
            }
        })).Message.ShouldBe("test exception");

        completed.ShouldBeFalse();
        failed.ShouldBeTrue();
        disposed.ShouldBeTrue();
    }

    [DataRow(true)]
    [DataRow(false)]
    [TestMethod]
    public async Task Should_Trigger_Failed_If_Rolled_Back(bool callComplete)
    {
        var completed = false;
        var failed = false;
        var disposed = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.OnCompleted(() =>
            {
                completed = true;
                return Task.CompletedTask;
            });
            uow.Failed += (sender, args) => { failed = true; args.IsRolledback.ShouldBeTrue(); };
            uow.Disposed += (sender, args) => disposed = true;

            await uow.RollbackAsync();

            if (callComplete)
            {
                await uow.CompleteAsync();
            }
        }

        completed.ShouldBeFalse();
        failed.ShouldBeTrue();
        disposed.ShouldBeTrue();
    }
}