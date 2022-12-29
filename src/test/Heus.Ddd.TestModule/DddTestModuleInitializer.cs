
using Heus.Core.DependencyInjection;
using Heus.Ddd.TestModule.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.TestModule;
[DependsOn(typeof(DddModuleInitializer))]
public class DddTestModuleInitializer: ModuleInitializerBase
{
    public async override Task InitializeAsync(IServiceProvider serviceProvider)
    {
      await  serviceProvider.PerformUowTask(async sp =>
      {
          var userAppService = sp.GetRequiredService<IUserAppService>();
          await userAppService.CreateAsync(new() { Name = "test1", Phone = "1310000000" });
          await userAppService.CreateAsync(new() { Name = "test2", Phone = "1320000000" });
      });
    }
}
