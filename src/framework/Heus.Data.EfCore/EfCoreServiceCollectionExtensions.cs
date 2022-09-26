

using Heus.Data.EfCore.Internal;
using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore
{
    public static class EfCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContextOptions(this IServiceCollection services
            , DbProvider dbProvider, Action<DbContextOptionsBuilder> action) {
            services.GetSingletonInstance<DbContextOptionsManager>().Register(dbProvider, action);
            return services;

            }
    }
}
