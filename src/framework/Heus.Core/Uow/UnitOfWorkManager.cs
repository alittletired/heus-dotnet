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
        var uowOptions = options ?? new UnitOfWorkOptions() { ServiceProvider= _serviceScopeFactory .CreateScope().ServiceProvider};
        var currentUow = _currentUow.Value;
        if (currentUow != null && !requiresNew)
        {
            return new ChildUnitOfWork(currentUow);
        }
        var unitOfWork = new UnitOfWork(uowOptions);
        _currentUow.Value = unitOfWork;
        unitOfWork.Disposed += (_, _) =>
        {
            _currentUow.Value = currentUow;
        };
        return unitOfWork;

    }
}