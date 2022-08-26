using System.Threading.Tasks;

namespace Heus.Ddd.Application.Services
{
    public interface IUpdateAppService<TUpdateOutput,  in TUpdateInput>
        : IApplicationService
    {
        Task<TUpdateOutput> UpdateAsync(TUpdateInput updateDto);
    }
}