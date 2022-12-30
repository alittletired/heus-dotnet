using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule.Domain;

namespace Heus.Ddd.Tests.Query;
public class UserAddressDto : User
{
    public string AddressCity { get; set; } = null!;
}
public class QueryExpressionVisitorComplexTest: DddIntegratedTest
{

    private readonly IRepository<UserAddress> _userAddressRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Address> _addressRepository;
    public QueryExpressionVisitorComplexTest() {
        _userRepository = GetRequiredService<IRepository<User>>();
        _addressRepository = GetRequiredService<IRepository<Address>>();
        _userAddressRepository = GetRequiredService<IRepository<UserAddress>>();
    }
    [Fact]
    public async Task InnerJoinQueryTest()
    {
        var query = from u in _userRepository.Query
                    join ua in _userAddressRepository.Query on u.Id equals ua.UserId
                    join a in _addressRepository.Query on ua.AddressId equals a.Id
                    select new { u, a };
        var search = new DynamicSearch<UserAddressDto>();
        search.AddEqualFilter(s => s.AddressCity, "武汉");

        var list = await query.ToPageListAsync(search);
        list.Total.ShouldBeGreaterThan(0);

    }
}
