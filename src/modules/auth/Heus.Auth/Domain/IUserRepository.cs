
using Heus.Core.Uow;

namespace Heus.Auth.Domain;

public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByNameAsync(string account);

}
