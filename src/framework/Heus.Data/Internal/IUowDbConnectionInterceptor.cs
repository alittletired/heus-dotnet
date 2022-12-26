
using System.Data.Common;

using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Heus.Data.Internal;
internal interface IUowDbConnectionInterceptor : IDbConnectionInterceptor
{

}

internal class UowDbConnectionInterceptor : DbConnectionInterceptor
    , IUowDbConnectionInterceptor,ISingletonDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UowDbConnectionInterceptor(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async override Task ConnectionOpenedAsync(DbConnection connection
        , ConnectionEndEventData eventData
        , CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null && _unitOfWorkManager.Current != null)
        {
            // await _unitOfWorkManager.Current.EnsureTransaction(eventData.Context);
        }
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

}
