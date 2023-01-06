using Heus.Core.Common;

namespace Heus.Core.Tests.Common;

public class ApiResultTests
{
    [Fact]
    public void Test_Error()
    {

        var result = ApiResult.FromException(new Exception("error"));
        result.Code.ShouldBe(500);
        result.Message.ShouldNotBeNull();
    }
    [Fact]
    public void Test_Ok()
    {
        var ok = ApiResult.Ok(new List<string>());
        ok.Code.ShouldBe(0);
        ok.Data.ShouldNotBeNull();
    }
}