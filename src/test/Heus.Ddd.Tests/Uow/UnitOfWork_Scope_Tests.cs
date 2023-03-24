using Heus.Core;
using Heus.Ddd.Uow;
using Heus.TestBase;

namespace Heus.Ddd.Tests.Uow;

public class UnitOfWork_Scope_Tests : IntegratedTestBase<DddTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    protected override bool AutoCreateUow => false;
    public UnitOfWork_Scope_Tests()
    {
        _unitOfWorkManager = UnitOfWorkManager;
    }

    [Fact]
    public  void UnitOfWork_Dispose_Twice()
    {
        var disposed = false;
        var uow = _unitOfWorkManager.Begin(ServiceProvider);
        uow.Disposed += (sender, args) => disposed = true;
        uow.Dispose();
        uow.Dispose();
        disposed.ShouldBeTrue();
    }
    [Fact]
    public async Task UnitOfWorkManager_Current_Should_Set_Correctly()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        using (var uow1 = _unitOfWorkManager.Begin(ServiceProvider))
        {
            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            using (var uow2 = _unitOfWorkManager.Begin(ServiceProvider))
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();
                await uow2.CompleteAsync();
            }

            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            await uow1.CompleteAsync();
        }

        _unitOfWorkManager.Current.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Create_Nested_UnitOfWorks()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        using (var uow1 = _unitOfWorkManager.Begin(ServiceProvider))
        {
            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            using (var uow2 = _unitOfWorkManager.Begin( ServiceProvider,requiresNew: true))
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();


                await uow2.CompleteAsync();
            }

            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            await uow1.CompleteAsync();
        }

        _unitOfWorkManager.Current.ShouldBeNull();
    }

}