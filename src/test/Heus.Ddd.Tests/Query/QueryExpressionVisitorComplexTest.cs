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
    [Theory]
    [InlineData("武汉", true)]
    [InlineData("异常", false)]
  
    public async Task ManyToManyJoinTest(string addressCity, bool hasResult)
    {
        var query = from u in _userRepository.Query
                    join ua in _userAddressRepository.Query on u.Id equals ua.UserId
                    join a in _addressRepository.Query on ua.AddressId equals a.Id
                    select new { u, a };
        var search = new DynamicSearch<UserAddressDto>();
        search.AddEqualFilter(s => s.AddressCity, addressCity);

        var result = await query.ToPageListAsync(search);
       
        if (hasResult)
        {
            result.Total.ShouldBeGreaterThan(0);
            result.Items.ForEach(i => i.AddressCity.ShouldBe(addressCity));
            result.Items.Count().ShouldBeGreaterThan(0);
        }
        else
        {
            result.Total.ShouldBe(0);
            result.Items.Count().ShouldBe(0);
        }
    }
    [Fact]
    public async Task Invalid_Type_Mapping_Should_Throw_Exception()
    {
        var query = from u in _userRepository.Query
                    join ua in _userAddressRepository.Query on u.Id equals ua.UserId
                    select new { u, ua };
        var search = new DynamicSearch<UserAddressDto>();
        search.AddEqualFilter(s => s.AddressCity, "武汉");
      await  Assert.ThrowsAsync<InvalidOperationException>(async () =>await query.ToPageListAsync(search));
    }

    [Theory]
    [InlineData("武汉", true)]
    //[InlineData("异常", false)]
    public async Task LeftJoinUsingWhereTest(string addressCity, bool hasResult)
    {
        var query = from u in _userRepository.Query
                    join ua in _userAddressRepository.Query on u.Id equals ua.UserId
                    from a in _addressRepository.Query.Where(a=> a.Id== ua.AddressId).DefaultIfEmpty()
                    where a.City== addressCity
                    select new { u, a };
        var search = new DynamicSearch<UserAddressDto>();
        //search.AddEqualFilter(s => s.AddressCity, addressCity);
        var result = await query.ToPageListAsync(search);
        if (hasResult)
        {
            result.Total.ShouldBeGreaterThan(0);
            result.Items.ForEach(i => i.AddressCity.ShouldBe(addressCity));
            result.Items.Count().ShouldBeGreaterThan(0);
        }
        else
        {
            result.Total.ShouldBe(0);
            result.Items.Count().ShouldBe(0);
        }

    }

    [Fact]
    public async Task OneToManyLeftJoinTest()
    {
        var query = from u in _userRepository.Query
            join ua in _userAddressRepository.Query on u.Id equals ua.UserId
            join a in _addressRepository.Query on ua.AddressId equals a.Id into  grouping
            from ua1 in grouping.DefaultIfEmpty()
            select new { u, ua1 };
        var search = new DynamicSearch<UserAddressDto>();
        search.AddEqualFilter(s => s.AddressCity, "武汉");

        var list = await query.ToPageListAsync(search);
        list.Total.ShouldBeGreaterThan(0);

    }
}
