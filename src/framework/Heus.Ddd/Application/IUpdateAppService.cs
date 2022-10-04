using System.Threading.Tasks;
using Heus.Ddd.Entities;

namespace Heus.Ddd.Application.Services
{
    public interface IUpdateAppService<  in TUpdateInput,TUpdateOutput>
        : IApplicationService
    {
        Task<TUpdateOutput> UpdateAsync(EntityId id, TUpdateInput updateDto);
    }
}