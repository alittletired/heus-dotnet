using Heus.Ddd.Dtos;
using Heus.Ddd.Repositories;
using Heus.Ddd.TestModule.Domain;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Heus.Ddd.Tests.Query;



public class QueryExpressionVisitorTest : DddIntegratedTest
{
  private readonly IRepository<User> _userRepository;
  public QueryExpressionVisitorTest()
  {
    _userRepository = GetRequiredService<IRepository<User>>();
    }
    [Theory]
    [InlineData("test1",true)]
    [InlineData("not_exists_test1", false)]
    public async Task AddEqualFilterTest(string name,bool hasResult) {
        var search = new DynamicSearch<User>();
        search.AddEqualFilter(s => s.Name, name);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total>0).ShouldBe(hasResult);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(200, false)]
    public async Task AddGreaterThanFilterTest(int sort, bool hasResult)
    {
        var search = new DynamicSearch<User>();
        search.AddGreaterThanFilter(s => s.Sort, sort);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total > 0).ShouldBe(hasResult);
    }
    [Theory]
    [InlineData(1, true)]
    [InlineData(200, true)]
    [InlineData(201,false)]
    public async Task AddGreaterOrEqualFilterTest(int sort, bool hasResult)
    {
        var search = new DynamicSearch<User>();
        search.AddGreaterOrEqualFilter(s => s.Sort, sort);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total > 0).ShouldBe(hasResult);
    }

    [Theory]
    [InlineData(200, true)]
    [InlineData(10, false)]
    public async Task AddLessThanFilterTest(int sort, bool hasResult)
    {
        var search = new DynamicSearch<User>();
        search.AddLessThanFilter(s => s.Sort, sort);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total > 0).ShouldBe(hasResult);
    }

    [Theory]
    [InlineData(200, true)]
    [InlineData(10, true)]
    public async Task AddLessOrEqualFilterTest(int sort, bool hasResult)
    {
        var search = new DynamicSearch<User>();
        search.AddLessOrEqualFilter(s => s.Sort, sort);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total > 0).ShouldBe(hasResult);
    }

    [Theory]
    [InlineData("test", true)]
    [InlineData("test1noexists", false)]
    public async Task AddHeadLikeFilterTest(string name , bool hasResult)
    {
        var search = new DynamicSearch<User>();
        search.AddHeadLikeFilter(s => s.Name, name);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total > 0).ShouldBe(hasResult);
    }
    [Theory]
    [InlineData("test1", true)]
    [InlineData("testnoexists", false)]
    public async Task AddTailLikeFilter(string name, bool hasResult)
    {
        var search = new DynamicSearch<User>();
        search.AddTailLikeFilter(s => s.Name, name);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total > 0).ShouldBe(hasResult);
    }
    [Theory]
    [InlineData("test1", true)]
    [InlineData("st", true)]
    [InlineData("testnoexists", false)]
    public async Task AddLikeFilterTest(string name, bool hasResult)
    {
        var search = new DynamicSearch<User>();
        search.AddLikeFilter(s => s.Name, name);
        var result = await _userRepository.Query.ToPageListAsync(search);
        (result.Total > 0).ShouldBe(hasResult);
    }


    [Theory]
    [InlineData(new int[] {100, 200})]
    [InlineData(new int[] { 300, 200 })]
    [InlineData(new int[] { 300, 400 })]
    public async Task AddInFilterTest(int[] sorts)
    {
        var allUsers = await _userRepository.FindAllAsync(s => true);
        var search = new DynamicSearch<User>();
        search.AddInFilter(s => s.Sort, sorts);
        var result = await _userRepository.Query.ToPageListAsync(search);
        var users = allUsers.Where(s => sorts.Contains(s.Sort));
        result.Total.ShouldBe(users.Count());
    }
    [Theory]
    [InlineData(new int[] {  200 })]
    [InlineData(new int[] { 100, 200 })]
    [InlineData(new int[] { 300, 200 })]
    [InlineData(new int[] { 300, 400 })]
    public async Task AddNotInFilterTest(int[] sorts)
    {
        var search = new DynamicSearch<User>();
        var allUsers = await _userRepository.FindAllAsync(s=>true);
        search.AddNotInFilter(s => s.Sort, sorts);
        var result = await _userRepository.Query.ToPageListAsync(search);
       var users= allUsers.Where(s => !sorts.Contains(s.Sort));
        result.Total.ShouldBe(users.Count());
    }
}