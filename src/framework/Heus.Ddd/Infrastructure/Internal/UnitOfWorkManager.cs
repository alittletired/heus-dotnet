using Heus.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.DDD.Infrastructure.Internal;

[Service(ServiceLifetime.Singleton)]
internal class UnitOfWorkManager:IUnitOfWorkManager
{
    private readonly AsyncLocal<IUnitOfWork?> _currentUow= new();
    public IUnitOfWork? Current => _currentUow.Value;

    public IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false)
    {
        var currentUow = _currentUow.Value;
        if (currentUow != null && !requiresNew)
        {
            return new ChildUnitOfWork(currentUow);
        }
        var unitOfWork = new UnitOfWork(options);
        _currentUow.Value = unitOfWork;
        unitOfWork.Disposed+=  (_, _) =>
        {
            _currentUow.Value = currentUow;
        };
        return unitOfWork;
        
    }
}