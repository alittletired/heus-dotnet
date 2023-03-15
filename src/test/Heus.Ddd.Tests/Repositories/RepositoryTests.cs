using Heus.Ddd.Domain;
using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule;
using Heus.Ddd.TestModule.Domain;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.Tests.Repositories;

public class RepositoryTests : DddIntegratedTest
{
    private readonly IRepository<User> _userRepository;

    public RepositoryTests()
    {
        _userRepository = GetRequiredService<IRepository<User>>();

    }

    [Fact]
    public async Task Insert_Delete_ManyAsync_Test()
    {
        var count = await _userRepository.Query.CountAsync();
        var users = new List<User>() {
            new() { Name = "InsertManyTest1", Phone = "1320000001" },
            new() { Name = "InsertManyTest2", Phone = "1320000002" },
            new() { Name = "InsertManyTest3", Phone = "1330000003" },
        };
        await ServiceProvider.PerformUowTask(async () =>
        {
            await _userRepository.InsertManyAsync(users);
        });
        var count1 = await _userRepository.Query.CountAsync();
       ( count1- count).ShouldBe(users.Count);
       await ServiceProvider.PerformUowTask(async () =>
       {
           await _userRepository.DeleteManyAsync(users);
       });
       var count2 = await _userRepository.Query.CountAsync();
       count2.ShouldBe(count);
       
    }
    [Fact]
    public async Task UpdateManyAsync_Test()
    {
        var users = await _userRepository.Query.AsNoTracking().ToListAsync();
       
        await ServiceProvider.PerformUowTask(async () =>
        {
            var users1 = await _userRepository.Query.AsNoTracking().ToListAsync();
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            await _userRepository.UpdateManyAsync(users1);
        });
        var users1 = await _userRepository.Query.AsNoTracking().ToListAsync();
        foreach (var user in users)
        {
            var user1 = users1.First(s => s.Id == user.Id);
            user1.UpdatedAt.Ticks.ShouldBeGreaterThan(user.UpdatedAt.Ticks);
        }

    }

    [Fact]
    public async Task GetById_ThrowError_When_NotExists()
    {
     var ex=  await Assert.ThrowsAsync<EntityNotFoundException>(
         async () =>await _userRepository.GetByIdAsync(-1));
     ex.Value.ShouldBe(-1);
     ex.EntityType.ShouldBe(typeof(User));
     ex.Property.ShouldBe(nameof(User.Id));
    }
    [Fact]
    public async Task Delete()
    {
        await _userRepository.DeleteByIdAsync(-1);
       
        await _userRepository.DeleteByIdAsync(MockData.Users[^1].Id);
    }
}