
using System.Data.Common;

using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Heus.Data.Internal;
internal interface IUnitofWorkDbConnectionInterceptor : IDbConnectionInterceptor
{

}
internal class UnitofWorkDbConnectionInterceptor : DbConnectionInterceptor, ISingletonDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    public UnitofWorkDbConnectionInterceptor(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }
    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null && _unitOfWorkManager.Current != null)
        {
            await _unitOfWorkManager.Current.EnsureTransaction(eventData.Context);
        }
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

}
