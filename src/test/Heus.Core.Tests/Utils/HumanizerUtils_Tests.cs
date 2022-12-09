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
    [Theory]
    [InlineData("some_title", "SomeTitle")]
    [InlineData("someTitle", "SomeTitle")]
    public void Pascalize_Test(string orig, string dest)
    {
        HumanizerUtils.Pascalize(orig).ShouldBe(dest);
        
    }
    [Theory]
    [InlineData("some_title", "someTitle")]
    [InlineData("SomeTitle", "someTitle")]
    public void Camelize_Test(string orig, string dest)
    {
        HumanizerUtils.Camelize(orig).ShouldBe(dest);
        
    }
    [Theory]
    [InlineData("someTitle", "some_title")]
    [InlineData("SomeTitle", "some_title")]
    public void Underscore_Test(string orig, string dest)
    {
        HumanizerUtils.Underscore(orig).ShouldBe(dest);
        
    }
    [Theory]
    [InlineData("someTitle", "some-title")]
    [InlineData("SomeTitle", "some-title")]
    [InlineData("some_title", "some-title")]
    public void Kebaberize_Test(string orig, string dest)
    {
        HumanizerUtils.Kebaberize(orig).ShouldBe(dest);
        
    }
}