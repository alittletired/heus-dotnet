
using Heus.Core.DependencyInjection;
using Heus.Ddd.TestModule.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.TestModule;
[DependsOn(typeof(DddModuleInitializer))]
public class DddTestModuleInitializer: ModuleInitializerBase
{
    public async override Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await serviceProvider.PerformUowTask(async sp =>
        {
            var authDbContext = sp.GetRequiredService<DddTestDbContext>();
            await authDbContext.Database.EnsureDeletedAsync();
            await authDbContext.Database.EnsureCreatedAsync();
        });
      await  serviceProvider.PerformUowTask(async sp =>
      {
          var userAppService = sp.GetRequiredService<IUserAdminAppService>();
          await userAppService.CreateAsync(new() {Id = 1,Name = "test1", Phone = "1310000000" });
          await userAppService.CreateAsync(new() {Id =2, Name = "test2", Phone = "1320000000" });
          await userAppService.CreateAsync(new() { Name = "test3", Phone = "1330000000" });
          await userAppService.CreateAsync(new() { Name = "test4", Phone = "1340000000" });
      });
    }
}
