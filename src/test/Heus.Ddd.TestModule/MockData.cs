
using Heus.Ddd.TestModule.Domain;

namespace Heus.Ddd.TestModule;

public static class MockData
{
    public const string UserName1 = "test1";
    public const string AddressCity1 = "北京";

    public static readonly List<User> Users = new List<User>()
    {
        new() { Id = 1, Name = "test1", Phone = "1310000000", Sort = 100 },
        new() { Id = 2, Name = "test2", Phone = "1320000000", Sort = 200 },
        new() { Id = 3, Name = "test3", Phone = "1330000000", Sort = 10 },
        new() { Id = 4,Name = "test4", Phone = "1340000000", Sort = 10 }

    };


    public static readonly List<Address> Addresses = new()
    {
        new() { Id = 11, City = "北京" },
        new() { Id = 12, City = "上海" }, new() { Id = 13, City = "武汉" }
    };

    public static readonly List<UserAddress> UserAddresses = new()
    {
        new() { AddressId = 11, UserId = 1 }, new() { AddressId = 12, UserId = 2 }, new() { AddressId = 13, UserId = 3 }
    };
}
