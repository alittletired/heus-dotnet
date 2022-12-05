using Heus.Auth.Application;
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
        await serviceProvider.PerformUowTask(async () =>
        {
            var authDbContext = serviceProvider.GetRequiredService<AuthDbContext>();
            await authDbContext.Database.EnsureDeletedAsync();
            await authDbContext.Database.EnsureCreatedAsync();
        });


    }


}
