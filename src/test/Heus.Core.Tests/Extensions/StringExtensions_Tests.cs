namespace Heus.Core.Tests.Extensions;

public class StringExtensions_Tests
{
    [Theory]
    [InlineData("a",true)]
    [InlineData("",false)]
    [InlineData(" ",true)]
    [InlineData(null,false)]
    public void HasText_Test(string? obj,bool expect)
    {
        obj.HasText().ShouldBe(expect);
    }

    [Theory]
    [InlineData("a", false)]
    [InlineData("", true)]
    [InlineData(" ", false)]
    [InlineData(null, true)]
    public void IsNullOrEmpty_Test(string? obj, bool expect)
    {
        obj.IsNullOrEmpty().ShouldBe(expect);
    }
    [Theory]
    [InlineData("a", false)]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData(null, true)]
    public void IsNullOrWhiteSpace_Test(string? obj, bool expect)
    {
        obj.IsNullOrWhiteSpace().ShouldBe(expect);
    }
    
}