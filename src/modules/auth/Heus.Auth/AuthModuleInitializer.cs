using Heus.Auth.Application;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Heus.Auth;
[ModuleDependsOn<DddModuleInitializer>]
public class AuthModuleInitializer : ModuleInitializerBase
{
    public override string? Name => "Auth";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IUserService, UserAdminAppService>();

    }

    public async override Task InitializeAsync(IServiceProvider serviceProvider)
    {
      //  await serviceProvider.PerformUowTask(async scope =>
      //  {
          //  var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
          //    await authDbContext.Database.EnsureDeletedAsync();
          
      //  });
        await serviceProvider.PerformUowTask(async (sp) =>
        {
            var dbContext = sp.GetRequiredService<AuthDbContext>();
            var databaseCreator = dbContext.GetService<IRelationalDatabaseCreator>();
            await databaseCreator.EnsureCreatedAsync();

            //await databaseCreator.CreateTablesAsync();
            //  await authDbContext.Database.EnsureDeletedAsync();
            
        });
        await serviceProvider.PerformUowTask(async sp =>
        {
           
            await SeedUsers(sp);
            await SeedRoles(sp);
        });

    }
    private async Task SeedUsers(IServiceProvider serviceProvider)
    {
        
        var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        var adminUser = await userRepository.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new User()
            {
                Name = "admin",
                Id = 1,
                IsSuperAdmin = true,
                NickName = "超级管理员",
                Phone = "13900000000"

            };
            adminUser.SetPassword("1");
            await userRepository.InsertAsync(adminUser);
        }
        
    }
    private  async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleRepository = serviceProvider.GetRequiredService<IRepository<Role>>();
        var roles = new List<Role>
        {
            new() { Id = 1,Name = "super_admin", Remarks = "超级管理员", IsBuildIn = true },
            new() { Id = 2,Name = "common_user", Remarks = "普通用户", IsBuildIn = true }
        };
        foreach (var role in roles)
        {
            if (!await roleRepository.ExistsAsync(r => r.Name == role.Name))
            {
                await roleRepository.InsertAsync(role);
            }
        }
    }


}
