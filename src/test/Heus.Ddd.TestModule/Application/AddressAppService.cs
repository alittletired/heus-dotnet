
using Heus.Ddd.Application;
using Heus.Ddd.TestModule.Domain;

namespace Heus.Ddd.TestModule.Application;
public interface IAddressAppService : IAdminApplicationService<Address> { }
public class AddressAppService : AdminApplicationService<Address>, IAddressAppService
{

}
