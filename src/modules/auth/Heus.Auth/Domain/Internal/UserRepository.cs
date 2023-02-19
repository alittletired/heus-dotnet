using Heus.Core.Uow;

namespace Heus.Auth.Domain.Internal;

internal class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(IUnitOfWorkManager unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<User?> FindByNameAsync(string account)
    {
        return await FindOneAsync(s => s.Name == account);
    }

   
}