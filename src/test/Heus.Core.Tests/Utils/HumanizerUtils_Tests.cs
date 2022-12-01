using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class HumanizerUtils_Tests
{
    [Theory]
    [InlineData("boy", "boys")]
    [InlineData("girl", "girls")]
    [InlineData("child", "children")]
    [InlineData("man", "men")]
    [InlineData("woman", "women")]
    [InlineData("bus", "buses")]
    [InlineData("box", "boxes")]
    public void Pluralize_Test(string orig, string dest)
    {
        HumanizerUtils.Pluralize(orig).ShouldBe(dest);
        HumanizerUtils.Singularize(dest).ShouldBe(orig);
    }
}