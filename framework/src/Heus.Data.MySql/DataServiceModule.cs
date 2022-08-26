using System.Data.Common;
using Heus.Business;
using Heus.Core;
using Heus.Core.Ddd.Data;
using Heus.Ddd.Data;
using Heus.DDD.Infrastructure;
using Heus.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Heus.Data.MySql;

[DependsOn(typeof(BusinessServiceModule))]
public class DataServiceModule : IServiceModule
{
    public void ConfigureServices(ConfigureServicesContext context)
    {
     
        context.Services.AddScoped<IDbContextProvider, DbContextProvider>();
        
        context.Services.AddDbContext<ApplicationDbContext>(options =>
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
    public void Configure(ConfigureContext context)
    {

        if (context.Environment.IsDevelopment())
        {
            ConfigureDevelopment(context);
        }

    }

    private static void ConfigureDevelopment(ConfigureContext context)
    {
        using var scope = context.ServiceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        var migrator = db.GetInfrastructure().GetService<IMigrator>()!;
        migrator.Migrate();
        // var str = migrator.GenerateScript();
        // db.Database.ExecuteSqlRaw(str);
    }
}
