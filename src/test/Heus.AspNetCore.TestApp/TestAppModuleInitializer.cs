using Heus.Core.DependencyInjection;

namespace Heus.AspNetCore.TestApp;

[DependsOn(typeof(AspNetModuleInitializer)
)]
public class TestAppModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public async override Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await serviceProvider.PerformUowTask(async sp =>
        {
            var authDbContext = sp.GetRequiredService<TestAppDbContext>();
            await authDbContext.Database.EnsureDeletedAsync();
            await authDbContext.Database.EnsureCreatedAsync();
        });
    }
}