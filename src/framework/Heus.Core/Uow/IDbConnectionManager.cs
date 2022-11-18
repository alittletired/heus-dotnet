using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Heus.Core.Uow;

public interface IDbConnectionManager
{
    DbConnection GetDbConnection();
}
