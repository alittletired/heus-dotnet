
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
          await userAppService.CreateAsync(new() {Id = 1,Name = "test1", Phone = "1310000000",Sort=100 });
          await userAppService.CreateAsync(new() {Id =2, Name = "test2", Phone = "1320000000", Sort = 200 });
          await userAppService.CreateAsync(new() { Id =3, Name = "test3", Phone = "1330000000" , Sort = 10 });
          await userAppService.CreateAsync(new() { Name = "test4", Phone = "1340000000", Sort = 10 });

          var userAddressRepo = sp.GetRequiredService<IRepository<UserAddress>>();
          var addressRepo = sp.GetRequiredService<IRepository<Address>>();
          await addressRepo.InsertAsync(new() { Id = 11, City = "北京" });
          await addressRepo.InsertAsync(new() { Id = 12, City = "上海" });
          await addressRepo.InsertAsync(new() { Id = 13, City = "武汉" });

          await userAddressRepo.InsertAsync(new UserAddress { AddressId = 11, UserId = 1 });
          await userAddressRepo.InsertAsync(new UserAddress { AddressId = 12, UserId = 2 });
          await userAddressRepo.InsertAsync(new UserAddress { AddressId = 13, UserId = 3 });
      });
    }
}
