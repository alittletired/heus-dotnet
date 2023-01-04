using Heus.AspNetCore.TestBase;

namespace Heus.AspNetCore.Tests.OpenApi;

public class OpenApiTest: AspNetIntegratedTest
{
    public OpenApiTest(WebApplicationFactory<AspNetCoreTestModule, Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task  Admin_Swagger_Should_Not_Empty()
    {
        var json = await _factory.HttpClient.GetStringAsync("/swagger/admin/swagger.json");
        json.ShouldNotBeEmpty();
      
    }
    [Fact]
    public async Task  Default_Swagger_Should_Not_Empty()
    {
        var json1 = await _factory.HttpClient.GetStringAsync("/swagger/default/swagger.json");
        // var json1 = await _factory.HttpClient.GetStringAsync("/swagger/index.html?urls.primaryName=admin");
        json1.ShouldNotBeEmpty();
    }

   
}