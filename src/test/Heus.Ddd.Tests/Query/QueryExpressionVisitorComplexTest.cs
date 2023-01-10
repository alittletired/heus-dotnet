using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heus.Ddd.Dtos;
using Heus.Ddd.Query;
using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule;
using Heus.Ddd.TestModule.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Heus.Ddd.Tests.Query;
public class UserAddressDto : User
{
    public string AddressCity { get; set; } = null!;
}
public class QueryExpressionVisitorComplexTest : DddIntegratedTest
{

    private readonly IRepository<UserAddress> _userAddressRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Address> _addressRepository;
    public QueryExpressionVisitorComplexTest()
    {
        _userRepository = GetRequiredService<IRepository<User>>();
        _addressRepository = GetRequiredService<IRepository<Address>>();
        _userAddressRepository = GetRequiredService<IRepository<UserAddress>>();
    }
    [Theory]
    [InlineData(nameof(UserAddressDto.Name), OperatorTypes.Equal, "test1", true)]
    [InlineData(nameof(UserAddressDto.Name), OperatorTypes.Equal, "test12222", false)]
    [InlineData(nameof(UserAddressDto.AddressCity), OperatorTypes.Equal, "武汉", true)]
    [InlineData(nameof(UserAddressDto.AddressCity), OperatorTypes.Equal, "异常", false)]

    public async Task ManyToManyJoinTest(string propName, string operatorType, string value, bool hasResult)
    {
        var query = from u in _userRepository.Query
                    join ua in _userAddressRepository.Query on u.Id equals ua.UserId
                    join a in _addressRepository.Query on ua.AddressId equals a.Id
                    where a.Id>0
                    select new { u, a };
        var search = new DynamicSearch<UserAddressDto>();
        search.AddFilter(propName, operatorType, value);
        var result = await query.ToPageListAsync(search);
        CheckResult(result, hasResult, propName, value);
    }
    [Fact]
    public async Task Invalid_Type_Mapping_Should_Throw_Exception()
    {
        var query = from u in _userRepository.Query
                    join ua in _userAddressRepository.Query on u.Id equals ua.UserId
                    select new { u, ua };
        var search = new DynamicSearch<UserAddressDto>();
        search.AddEqualFilter(s => s.AddressCity, "武汉");
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await query.ToPageListAsync(search));
    }

    private void CheckResult<TDto>(PageList<TDto> result, bool hasResult, string propName, string value)
    {
        var prop = typeof(UserAddressDto).GetProperty(propName)!;
      
        if (hasResult)
        {
            result.Total.ShouldBeGreaterThan(0);
            result.Items.ForEach(i => prop.GetValue(i).ShouldBe(value));
            result.Items.Count().ShouldBeGreaterThan(0);
        }
        else
        {
            result.Total.ShouldBe(0);
            result.Items.Count().ShouldBe(0);
        }
    }

    [Theory]
    [InlineData(nameof(UserAddressDto.Name), OperatorTypes.Equal, MockData.UserName1, true)]
    [InlineData(nameof(UserAddressDto.Name), OperatorTypes.Equal, MockData.UserName1 + "test12222", false)]
    [InlineData(nameof(UserAddressDto.AddressCity), OperatorTypes.Equal, MockData.AddressCity1, true)]
    [InlineData(nameof(UserAddressDto.AddressCity), OperatorTypes.Equal, MockData.AddressCity1 + "异常", false)]
    public async Task LeftJoinUsingWhereTest(string propName, string operatorType, string value, bool hasResult)
    {
        var query = from u in _userRepository.Query
            join ua in _userAddressRepository.Query on u.Id equals ua.UserId
            from a in _addressRepository.Query.Where(a => a.Id == ua.AddressId).DefaultIfEmpty()
            select new { u, a };

        await LeftJoinCheck(query, propName, operatorType, value, hasResult);


        query = from u in _userRepository.Query
            join ua in _userAddressRepository.Query on u.Id equals ua.UserId
            from a in _addressRepository.Query.Where(a => a.Id == ua.AddressId).DefaultIfEmpty()
            where a.City != ""
            orderby u.Id descending 
            select new { u, a };

        await LeftJoinCheck(query, propName, operatorType, value, hasResult);

        var query1 = from u in _userRepository.Query
            join ua in _userAddressRepository.Query on u.Id equals ua.UserId
            join a in _addressRepository.Query on ua.AddressId equals a.Id into gj
            from a1 in gj.DefaultIfEmpty()
            where a1.City != ""
            select new { u, a1 };

        await LeftJoinCheck(query1, propName, operatorType, value, hasResult);
        var query2 = from u in _userRepository.Query
            join ua in _userAddressRepository.Query on u.Id equals ua.UserId
            join a in _addressRepository.Query on ua.AddressId equals a.Id into gj
            from a1 in gj.DefaultIfEmpty()
            select new { u, a1 };

        await LeftJoinCheck(query2, propName, operatorType, value, hasResult);

    }

    private async Task LeftJoinCheck<TSource>(IQueryable<TSource> query,
        string propName, string operatorType, string value, bool hasResult)
    {
        var search = new DynamicSearch<UserAddressDto>();
        search.AddFilter(propName, operatorType, value);
        var result = await query.ToPageListAsync(search);
        CheckResult(result, hasResult, propName, value);
    }
    

}
