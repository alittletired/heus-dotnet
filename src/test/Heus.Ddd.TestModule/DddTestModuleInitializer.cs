
using Heus.Core.DependencyInjection;
using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule.Application;
using Microsoft.Extensions.DependencyInjection;
using Heus.Ddd.TestModule.Domain;
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
          await MockData.Users.ForEachAsync(userAppService.CreateAsync);
          //await userAppService.CreateAsync(MockData.User1);
          //await userAppService.CreateAsync(MockData.User2);
          //await userAppService.CreateAsync(MockData.User3);
          //await userAppService.CreateAsync(MockData.User4);
  

          var userAddressRepo = sp.GetRequiredService<IRepository<UserAddress>>();
          var addressRepo = sp.GetRequiredService<IRepository<Address>>();
          await MockData.Addresses.ForEachAsync(addressRepo.InsertAsync);

          await MockData.UserAddresses.ForEachAsync(userAddressRepo.InsertAsync);
         
      });
    }
}
