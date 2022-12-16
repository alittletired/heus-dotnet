using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Uow;
internal class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly AsyncLocal<IUnitOfWork?> _currentUow = new();
    public IUnitOfWork? Current => _currentUow.Value;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UnitOfWorkManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    public IUnitOfWork Begin(UnitOfWorkOptions? options=default, bool requiresNew = false)
    {

        var uowOptions = options ?? new UnitOfWorkOptions() ;
        var currentUow = _currentUow.Value;
        if (currentUow != null && !requiresNew)
        {
            return new ChildUnitOfWork(currentUow);
        }

        var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = new UnitOfWork(scope.ServiceProvider, uowOptions);
        _currentUow.Value = unitOfWork;
        unitOfWork.Disposed += (_, _) =>
        {
            _currentUow.Value = currentUow;
            scope.Dispose();
        };
        return unitOfWork;

    }
}