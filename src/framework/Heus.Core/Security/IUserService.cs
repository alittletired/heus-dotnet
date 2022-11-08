namespace Heus.Core.Security;

public interface IUserService
{
    Task<ICurrentUser?> FindByNameAsync(string name);
}