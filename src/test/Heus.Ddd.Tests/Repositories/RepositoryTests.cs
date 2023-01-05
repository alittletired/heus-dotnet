using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule.Domain;
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
    public async Task InsertManyAsync_Test()
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
            user1.UpdateDate.Ticks.ShouldBeGreaterThan(user.UpdateDate.Ticks);
        }

    }
}