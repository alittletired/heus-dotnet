using Heus.Data.EfCore.ValueConverters;

namespace Heus.Data;
public class DataConfigurationOptions
{
    public DbProvider DbProvider { get; set; } = DbProvider.PostgreSql;
    public List<IDbConnectionProvider> DbConnectionProviders { get;  }=new ();

    internal static readonly Action<ModelConfigurationBuilder> DefaultModelConfiguration = (options) =>
    {
        var modelBuilder = options.CreateModelBuilder(null);
        var propertyTypes = modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.ClrType.GetProperties())
            .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(EnumClass<>))
            .Select(p => p.PropertyType)
            .Distinct();

        foreach (var propertyType in propertyTypes)
        {
            var converterType = typeof(EnumValueConverter<>).MakeGenericType(propertyType);
            options.Properties(propertyType)
                .HaveConversion(converterType);
        }
        //options.Properties<long>()
        // .HaveConversion<longConverter>().HaveMaxLength(24).AreUnicode(false);
    };


    public List<Action<ModelConfigurationBuilder>> ModelConfiguration = new() { DefaultModelConfiguration };

    public Dictionary<Type, Type> EntityDbContextMappings { get; } = new();

}
