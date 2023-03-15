

namespace Heus.Ddd.Uow;
internal class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly AsyncLocal<IUnitOfWork?> _currentUow = new();
    public IUnitOfWork? Current => _currentUow.Value;
    

    
    public IUnitOfWork Begin(IServiceProvider serviceProvider, UnitOfWorkOptions? options=default, bool requiresNew = false)
    {

        var uowOptions = options ?? new UnitOfWorkOptions() ;
        var currentUow = _currentUow.Value;
        if (currentUow != null && !requiresNew)
        {
            return new ChildUnitOfWork(currentUow);
        }
       
        var unitOfWork = new UnitOfWork(serviceProvider, uowOptions);
        _currentUow.Value = unitOfWork;
        unitOfWork.Disposed += (_, _) =>
        {
            _currentUow.Value = currentUow;
          
        };
        return unitOfWork;

    }
}