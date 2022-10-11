using Heus.Core.DependencyInjection;
using System.Security.Claims;
using Heus.Ddd.Entities;

namespace Heus.Core.Security;
public interface ICurrentUser
{
    EntityId Id { get; }
    string UserName { get; }
 
}
