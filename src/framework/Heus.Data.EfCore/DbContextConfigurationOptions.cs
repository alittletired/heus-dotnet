using Heus.Core;
using Heus.Core.Data;
using Heus.Core.Data.Options;
using Heus.Data.EfCore.ValueConverters;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;


namespace Heus.Data.EfCore;

public class DbContextConfigurationOptions
{
    internal static readonly Action<DbContextOptionsBuilder> DefaultConfigureAction= (options) =>
    {
        options.UseSnakeCaseNamingConvention();
    };
    internal static readonly Action<ModelConfigurationBuilder> DefaultModelConfiguration = (options) => {
        var modelBuilder = options.CreateModelBuilder(null);
        var propertyTypes = modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.ClrType.GetProperties())
            .Where(p =>p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition()== typeof(EnumClass<>))
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

    public  List<Action<DbContextOptionsBuilder>> DbContextOptionsActions { get; }= new() { DefaultConfigureAction };
    public List<Action<ModelConfigurationBuilder>> ModelConfiguration = new() { DefaultModelConfiguration };
    public DbProvider? DefaultDbProvider { get; set; }
    public Dictionary<Type, Type> EntityDbContextMappings { get; } = new();

}
