using Heus.Core.Uow;
using Heus.TestBase;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Tests.Uow;

public class UnitOfWork_Scope_Tests : IntegratedTestBase<CoreModuleInitializer>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    protected override bool AutoCreateUow => false;
    public UnitOfWork_Scope_Tests()
    {
        _unitOfWorkManager = RootServiceProvider.GetRequiredService<IUnitOfWorkManager>(); ;
    }
    [Fact]
    public async Task UnitOfWorkManager_Current_Should_Set_Correctly()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        using (var uow1 = _unitOfWorkManager.Begin())
        {
            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            using (var uow2 = _unitOfWorkManager.Begin())
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

        using (var uow1 = _unitOfWorkManager.Begin())
        {
            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            using (var uow2 = _unitOfWorkManager.Begin(requiresNew: true))
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