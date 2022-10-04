using System.Data.Common;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Data.ValueConversion;
using Heus.Ddd.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Data;

using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;
public abstract class DbContextBase<TDbContext> : DbContext,IInjectServiceProvider,IScopedDependency
    where TDbContext : DbContext
{
   public IServiceProvider ServiceProvider { get; set; } = null!;
    protected DbContextBase(DbContextOptions<TDbContext> options)
      : base(options)
    {
    }
    protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

   protected override void OnConfiguring(DbContextOptionsBuilder options)
   {
      options.UseSnakeCaseNamingConvention();
      
      base.OnConfiguring(options);
   }

   protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
   {
      configurationBuilder
         .Properties<EntityId>()
         .HaveConversion<EntityIdConverter>().HaveMaxLength(20).AreUnicode(false);
   }
  
}