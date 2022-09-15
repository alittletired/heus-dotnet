using Heus.Core.Ddd.Data;
using Heus.Ddd.Data.ValueConversion;
namespace Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
public abstract class DbContextBase<TDbContext> : DbContext
   where TDbContext : DbContext
{
   protected DbContextBase(DbContextOptions<TDbContext> options)
      : base(options)
   {
   }
   protected override void OnConfiguring(DbContextOptionsBuilder options)
   {
      options.UseSnakeCaseNamingConvention();
      base.OnConfiguring(options);
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
     
   }
 

   protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
   {
      configurationBuilder
         .Properties<EntityId>()
         .HaveConversion<EntityIdConverter>().HaveMaxLength(20).AreUnicode(false);
   }
}