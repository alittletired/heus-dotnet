using Heus.Data.EfCore.ValueConverters;
namespace Heus.Data;
public  interface IDbContext{}
public abstract class DbContextBase<TDbContext> : DbContext,IDbContext
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
           .Where(p => IsDerived(p.PropertyType, typeof(EnumClass<>)))
           .Select(p => p.PropertyType)
           .Distinct();

       foreach (var propertyType in propertyTypes)
       {
           var converterType = typeof(EnumClassValueConverter<>).MakeGenericType(propertyType);
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