using Heus.Core.Common;
using Heus.Data.EfCore.ValueConverters;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Heus.Data;
public class DataOptions
{
   
    public List<IDbConnectionProvider> DbConnectionProviders { get;  }=new ();
    public List<IInterceptor> Interceptors { get; } = new();
    public List<Action<DbContextOptionsBuilder>> ConfigureDbContextOptions { get; }

    public DataOptions()
    {
        ModelConfigurations = new() { DefaultModelConfiguration };
        ConfigureDbContextOptions = new() {
            (builder) =>
            {
                //var logLevel = LogLevel.Information;
                //#if DEBUG
//        logLevel = LogLevel.Debug;
//#endif
                builder.UseSnakeCaseNamingConvention();
                builder.LogTo(Console.WriteLine
                        //,LogLevel.Debug)
                        //,new[] { DbLoggerCategory.Database.Name })
                        , (eventId, logLevel) => logLevel >= LogLevel.Information
                                                 || eventId == RelationalEventId.ConnectionOpened
                                                 || eventId == RelationalEventId.ConnectionClosed
                                                 || eventId == RelationalEventId.TransactionStarted
                                                 || eventId == RelationalEventId.TransactionRolledBack
                                                 || eventId == RelationalEventId.TransactionCommitted
                                                 || eventId == RelationalEventId.TransactionDisposed
                    )
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        };
    }

    internal static readonly Action<ModelConfigurationBuilder> DefaultModelConfiguration = (options) =>
    {
        var modelBuilder = options.CreateModelBuilder(null);
        var propertyTypes = modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.ClrType.GetProperties())
            .Where(p => p.PropertyType.IsGenericType 
                        && p.PropertyType.GetGenericTypeDefinition() == typeof(EnumClass<>))
            .Select(p => p.PropertyType)
            .Distinct();

        foreach (var propertyType in propertyTypes)
        {
            var converterType = typeof(EnumClassValueConverter<>).MakeGenericType(propertyType);
            options.Properties(propertyType)
                .HaveConversion(converterType);
        }
        //options.Properties<long>()
        // .HaveConversion<longConverter>().HaveMaxLength(24).AreUnicode(false);
    };


    public List<Action<ModelConfigurationBuilder>> ModelConfigurations { get; } 
   

}
