using Heus.Auth.Entities;
using Heus.Ddd.Repositories;

namespace Heus.Auth.Domain;

public interface IUserRepository : IRepository<User>
{
   Task<User?> FindByAccountAsync(string account);

}
internal class UserRepository:RepositoryBase<User>,IUserRepository
{
   public async Task<User?> FindByAccountAsync(string account)
   {
      return await FindAsync(s => string.Equals(s.Account, account, StringComparison.OrdinalIgnoreCase));
   }
}