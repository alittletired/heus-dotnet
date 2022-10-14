using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace Heus.Ddd.Data;
public abstract class DbContextBase<TDbContext> : DbContext,IScopedDependency
    where TDbContext : DbContext
{
  
    protected DbContextBase(DbContextOptions<TDbContext> options)
      : base(options)
    {
    }


   protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
   {
      //configurationBuilder
      //   .Properties<long>()
      //   .HaveConversion<longConverter>().HaveMaxLength(24).AreUnicode(false);
   }
  
}