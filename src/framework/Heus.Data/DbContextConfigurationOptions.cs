using Heus.Data.Options;
using Heus.Data.EfCore.ValueConverters;
using Microsoft.Extensions.Logging;
namespace Heus.Data;
public class DbContextConfigurationOptions
{
    public DbProvider DbProvider { get; set; } = DbProvider.PostgreSql;
    public IEnumerable<IDbConnectionProvider> ConnectionProviders { get;  }=new List<IDbConnectionProvider>();
internal static Action<DbContextOptionsBuilder> DefaultConfigureAction => (options) =>
    {
        var logLevel = LogLevel.Information;
#if DEBUG
        logLevel = LogLevel.Debug;
#endif
        options.UseSnakeCaseNamingConvention();
        options.LogTo(Console.WriteLine, logLevel)
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors();
    };
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

    public List<Action<DbContextOptionsBuilder>> DbContextOptionsActions { get; } = new() { DefaultConfigureAction };
    public List<Action<ModelConfigurationBuilder>> ModelConfiguration = new() { DefaultModelConfiguration };

    public Dictionary<Type, Type> EntityDbContextMappings { get; } = new();

}
