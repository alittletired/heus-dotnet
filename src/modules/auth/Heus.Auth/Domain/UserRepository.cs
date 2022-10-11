using Heus.Auth.Entities;
using Heus.Ddd.Repositories;

namespace Heus.Auth.Domain;

public interface IUserRepository : IRepository<User>
{
   Task<User?> FindByUserNameAsync(string account);

}
internal class UserRepository:RepositoryBase<User>,IUserRepository
{
   public async Task<User?> FindByUserNameAsync(string account)
   {
      return await FindAsync(s => s.UserName== account);
   }
}