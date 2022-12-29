
using Heus.Ddd.Application;
using Heus.Ddd.TestModule.Domain;

namespace Heus.Ddd.TestModule.Application;
public interface IUserAdminAppService : IAdminApplicationService<User> { }
internal class UserAdminAppService : AdminApplicationService<User>, IUserAdminAppService
{

}
