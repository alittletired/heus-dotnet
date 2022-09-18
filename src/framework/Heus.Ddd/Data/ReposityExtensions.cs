namespace Heus.Ddd.Data;

using Microsoft.EntityFrameworkCore;

public static class RepositoryExtensions
{
    public static async Task<T> GetByIdAsync<T>(IRepository<T> repository, EntityId id) where T : IEntity
    {

        return await repository.Query.FirstAsync(s => s.Id == id);
    }

    public static async Task<T?> GetByIdOrDefaultAsync<T>(IRepository<T> repository, EntityId id) where T : IEntity
    {
        return await repository.Query.FirstOrDefaultAsync(s => s.Id == id);

    }
}