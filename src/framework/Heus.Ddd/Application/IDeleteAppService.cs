using Heus.Ddd.Entities;

namespace Heus.Ddd.Application.Services;
    public interface IDeleteAppService : IApplicationService
    {
        Task DeleteAsync(EntityId id);
    }
