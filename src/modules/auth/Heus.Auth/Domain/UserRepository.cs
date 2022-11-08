using Heus.Auth.Entities;
using Heus.Ddd.Repositories;

namespace Heus.Auth.Domain;

public interface IUserRepository : IRepository<User>
{
   Task<User?> FindByNameAsync(string account);

}
internal class UserRepository:RepositoryBase<User>,IUserRepository
{
   public async Task<User?> FindByNameAsync(string account)
   {
      return await FindAsync(s => s.Name== account);
   }
}