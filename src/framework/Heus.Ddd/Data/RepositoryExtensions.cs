using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Ddd.Data
{
    public static class RepositoryExtensions
    {
        public static  async Task<TEntity> GetByIdAsync<TEntity>(IRepository<TEntity> repository, EntityId id) where TEntity : class, IEntity
        {
            var query = await repository.GetQueryableAsync();
            return await query.FirstAsync(s => s.Id == id);
        }

        public static async Task<TEntity?> GetByIdOrDefaultAsync<TEntity>(IRepository<TEntity> repository, EntityId id) where TEntity : class, IEntity
        {
            var query = await repository.GetQueryableAsync();
            return await query.FirstOrDefaultAsync(s => s.Id == id);

        }
    }
}
