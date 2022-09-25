using System.Data.Common;
using Heus.Ddd.Data.ValueConversion;
namespace Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;
public abstract class DbContextBase: DbContext
{
   private readonly DbConnection _connection;

   protected DbContextBase(DbConnection connection)
   {
      _connection = connection;
   }

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