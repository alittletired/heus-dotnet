namespace Heus.Auth.Domain.Internal;

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