using Heus.Core.Ddd.Data;
using Heus.Ddd.Data.ValueConversion;
namespace Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
public abstract class DbContextBase: DbContext
{
 
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