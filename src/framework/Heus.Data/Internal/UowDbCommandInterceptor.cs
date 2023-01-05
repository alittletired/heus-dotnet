// using System.Data.Common;
// using Heus.Core.DependencyInjection;
// using Microsoft.EntityFrameworkCore.Diagnostics;
// using Microsoft.EntityFrameworkCore.Storage;
//
// namespace Heus.Data.Internal;
//
// public interface IUowDbCommandInterceptor:IDbCommandInterceptor
// {
//     
// }
//
// public class UowDbCommandInterceptor : DbCommandInterceptor, IUowDbCommandInterceptor, ISingletonDependency
// {
//     public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
//     {
//         var transaction  = eventData.Context?.Database.CurrentTransaction;
//         if (transaction != null &&  result.Transaction ==null)
//         {
//             // result.Transaction = transaction.GetDbTransaction();
//         }
//         return base.CommandCreated(eventData, result);
//     }
// }