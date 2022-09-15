using Heus.Business;
using Heus.Core.Ddd.Data;
using Heus.Core.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Heus.Data.MySql;

[DependsOn(typeof(BusinessServiceModule))]
public class DataServiceModule : ServiceModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        var services = context.Services;
        services.AddScoped<IDbContextProvider, DbContextProvider>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        { 
            var connectionString = context.Configuration.GetConnectionString(nameof(ApplicationDbContext));
           var version= ServerVersionCache.GetServerVersion(connectionString);
           options.UseMySql(connectionString, version)
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
        });


    }

    // public void ConfigureServicesDevelopment(ConfigureServicesContext context)
    // {
    //     context.Services.AddDbContext<ApplicationDbContext>(options =>
    //     {
    //         
    //         var folder = Environment.SpecialFolder.LocalApplicationData;
    //         var path = Environment.GetFolderPath(folder);
    //         var dbPath = Path.Join(path, "application.db");
    //         // _logger.LogInformation("dbPath:{DbPath}",dbPath);
    //         options.UseSqlite($"Data Source={dbPath}");
    //         
    //     
    //
    //     });
    // }
    public override void ConfigureApplication(ApplicationConfigurationContext context)
    {
        if (context.Environment.IsDevelopment())
        {
            ConfigureDevelopment(context.ServiceProvider);
        }
    }

    private static void ConfigureDevelopment(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        var migrator = db.GetInfrastructure().GetService<IMigrator>()!;
        migrator.Migrate();
        // var str = migrator.GenerateScript();
        // db.Database.ExecuteSqlRaw(str);
    }
}
