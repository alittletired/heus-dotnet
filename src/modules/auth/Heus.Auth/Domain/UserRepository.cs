
using Heus.Core.Uow;

namespace Heus.Auth.Domain;

public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByNameAsync(string account);

}
internal class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task<User?> FindByNameAsync(string account)
    {
        return await FindOneAsync(s => s.Name == account);
    }
}