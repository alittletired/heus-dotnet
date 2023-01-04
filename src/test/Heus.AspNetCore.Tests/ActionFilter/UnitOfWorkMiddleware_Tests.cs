

using Heus.AspNetCore.TestBase;

namespace Heus.AspNetCore.Tests.ActionFilter;
public class UnitOfWorkMiddleware_Tests : AspNetIntegratedTest
{
  
    public UnitOfWorkMiddleware_Tests(WebApplicationFactory<AspNetCoreTestModule, Program> factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Get_Actions_Should_Not_Be_Transactional()
    {
        var res=await _factory.HttpClient.GetAsync("/api/unitofwork-test/UowWithoutTransaction");
        res.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task Non_Get_Actions_Should_Be_Transactional()
    {
        var result = await _factory.HttpClient.PostAsync("/api/unitofwork-test/UowWithTransaction", null);
        result.IsSuccessStatusCode.ShouldBeTrue();
    }

   
}
