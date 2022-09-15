using Heus.Business;
using Heus.Core.Ddd.Data;
using Heus.DDD.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.MySql;

public class DbContextProvider:IDbContextProvider
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUnitOfWorkManager _unitOfWorkManager; 
    public DbContextProvider(ApplicationDbContext applicationDbContext, IUnitOfWorkManager unitOfWorkManager)
    {
        _applicationDbContext = applicationDbContext;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public DbContext GetDbContext(Type entityType)
    {
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new Exception("A DbContext can only be created inside a unit of work!");
        }
        if (_unitOfWorkManager.Current?.Options.IsTransactional == true
            &&  _applicationDbContext.Database.CurrentTransaction==null)
        {
           
         _applicationDbContext.Database.BeginTransaction();
        }
        return _applicationDbContext;
    }
}