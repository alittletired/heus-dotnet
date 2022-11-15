using Heus.Auth.Application;
using Heus.Auth.Domain;
using Heus.Auth.Entities;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;
namespace Heus.Auth;

[DependsOn(typeof(DddModuleInitializer))]
public class AuthModuleInitializer : ModuleInitializerBase
{
    public override string? Name => "Auth";
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IUserService, UserAdminAppService>();

    }

    public override async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var authDbContext = serviceProvider.GetRequiredService<AuthDbContext>();
        await authDbContext.Database.EnsureDeletedAsync();
        await authDbContext.Database.EnsureCreatedAsync();
        await AddUsers(serviceProvider);
        await AddRoles(serviceProvider);
    }

    private async Task AddUsers(IServiceProvider serviceProvider)
    {
        var userService = serviceProvider.GetRequiredService<IUserAdminAppService>();
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

    private async Task AddRoles(IServiceProvider serviceProvider)
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
