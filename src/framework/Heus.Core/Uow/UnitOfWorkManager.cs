
namespace Heus.Core.Uow;

internal class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly AsyncLocal<IUnitOfWork?> _currentUow = new();
    public IUnitOfWork? Current => _currentUow.Value;
    private readonly ILoggerFactory _loggerFactory;
    public UnitOfWorkManager(ILoggerFactory loggerFactory)
    {

        _loggerFactory = loggerFactory;
    }

    public IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false)
    {
        var currentUow = _currentUow.Value;
        if (currentUow != null && !requiresNew)
        {
            return new ChildUnitOfWork(currentUow);
        }
        var unitOfWork = new UnitOfWork(options, _loggerFactory.CreateLogger<UnitOfWork>());
        _currentUow.Value = unitOfWork;
        unitOfWork.Disposed += (_, _) =>
        {
            _currentUow.Value = currentUow;
        };
        return unitOfWork;

    }
}