
using Heus.Ddd.Application;
using Heus.Ddd.TestModule.Domain;

namespace Heus.Ddd.TestModule.Application;
public interface IUserAppService : IAdminApplicationService<User> { }
public class PeopleAppService : AdminApplicationService<User>, IUserAppService
{

}
