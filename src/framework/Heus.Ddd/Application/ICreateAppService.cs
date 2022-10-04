
namespace Heus.Ddd.Application;
public interface ICreateAppService<in TCreateInput,TCreateOut> : IApplicationService
{
    Task<TCreateOut> CreateAsync(TCreateInput createDto);
}
