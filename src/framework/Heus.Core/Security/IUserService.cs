namespace Heus.Core.Security;

public interface IUserService
{
    Task<ICurrentUser?> FindByUserNameAsync(string name);
}