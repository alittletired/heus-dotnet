using Heus.Core;
using Heus.Core.DependencyInjection;
using Heus.Data.EfCore.ValueConverters;
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
       var modelBuilder = configurationBuilder.CreateModelBuilder(null);
       var propertyTypes = modelBuilder.Model.GetEntityTypes()
           .SelectMany(e => e.ClrType.GetProperties())
           .Where(p => IsDerived(p.PropertyType, typeof(EnumBase<>)))
           .Select(p => p.PropertyType)
           .Distinct();

       foreach (var propertyType in propertyTypes)
       {
           
           var converterType = typeof(EnumValueConverter<>).MakeGenericType(propertyType);

           configurationBuilder.Properties(propertyType)
               .HaveConversion(converterType);
       }
      //configurationBuilder
      //   .Properties<long>()
      //   .HaveConversion<longConverter>().HaveMaxLength(24).AreUnicode(false);
   }
   public static bool IsDerived(Type objectType, Type mainType)
   {
       Type? currentType = objectType.BaseType;

       if (currentType == null)
       {
           return false;
       }

       while (currentType != typeof(object))
       {
           if (currentType?.IsGenericType==true && currentType.GetGenericTypeDefinition() == mainType)
               return true;

           currentType = currentType?.BaseType;
       }

       return false;
   }
  
}