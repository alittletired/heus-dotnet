using Heus.Ddd.Entities;

namespace Heus.Ddd.Application.Services;
    public interface IDeleteAppService : IApplicationService
    {
        Task<long> DeleteAsync(long id);
    }
