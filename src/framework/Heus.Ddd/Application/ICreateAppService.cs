
namespace Heus.Ddd.Application;
public interface ICreateAppService<in TCreateInput,TCreateOut> : IAppService
{
    Task<TCreateOut> CreateAsync(TCreateInput createDto);
}
