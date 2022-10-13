using Heus.Core.Data;
using Heus.Core.Data.Options;
using Heus.Ddd.Data.ValueConversion;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;


namespace Heus.Data.EfCore;

public class DbContextConfigurationOptions
{
    internal static Action<DbContextOptionsBuilder> DefaultConfigureAction = (options) => { options.UseSnakeCaseNamingConvention(); };
    internal static Action<ModelConfigurationBuilder> DefaultConventionAction = (options) => {
        options.Properties<EntityId>()
         .HaveConversion<EntityIdConverter>().HaveMaxLength(24).AreUnicode(false);
    };

    public List<Action<DbContextOptionsBuilder>> ConfigureActions = new() { DefaultConfigureAction };
    public List<Action<ModelConfigurationBuilder>> ConventionActions = new() { DefaultConventionAction };
    public DbProvider DefaultDbProvider { get; set; } = DbProvider.MySql;


   


}
