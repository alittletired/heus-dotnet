
using Heus.Ddd.Data;

namespace Heus.Ddd.Application.Services;
    public interface IDeleteAppService : IApplicationService
    {
        Task DeleteAsync(EntityId id);
    }
