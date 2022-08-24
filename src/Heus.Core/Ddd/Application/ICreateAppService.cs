
namespace Heus.Ddd.Application;
public interface ICreateAppService<TCreateOut, in TCreateInput> : IApplicationService
{
    Task<TCreateOut> CreateAsync(TCreateInput createDto);
}
