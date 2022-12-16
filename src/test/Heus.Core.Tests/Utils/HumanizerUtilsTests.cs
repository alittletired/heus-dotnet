using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;
[TestClass]
public class HumanizerUtilsTests
{
    [TestMethod]
    [DataRow("boy", "boys")]
    [DataRow("girl", "girls")]
    [DataRow("child", "children")]
    [DataRow("man", "men")]
    [DataRow("woman", "women")]
    [DataRow("bus", "buses")]
    [DataRow("box", "boxes")]
    public void Pluralize_Test(string orig, string dest)
    {
        HumanizerUtils.Pluralize(orig).ShouldBe(dest);
        HumanizerUtils.Singularize(dest).ShouldBe(orig);
    }
    [TestMethod]
    [DataRow("some_title", "SomeTitle")]
    [DataRow("someTitle", "SomeTitle")]
    public void Pascalize_Test(string orig, string dest)
    {
        HumanizerUtils.Pascalize(orig).ShouldBe(dest);

    }
    [TestMethod]
    [DataRow("some_title", "someTitle")]
    [DataRow("SomeTitle", "someTitle")]
    public void Camelize_Test(string orig, string dest)
    {
        HumanizerUtils.Camelize(orig).ShouldBe(dest);

    }
    [TestMethod]
    [DataRow("someTitle", "some_title")]
    [DataRow("SomeTitle", "some_title")]
    public void Underscore_Test(string orig, string dest)
    {
        HumanizerUtils.Underscore(orig).ShouldBe(dest);

    }
    [TestMethod]
    [DataRow("someTitle", "some-title")]
    [DataRow("SomeTitle", "some-title")]
    [DataRow("some_title", "some-title")]
    public void Kebaberize_Test(string orig, string dest)
    {
        HumanizerUtils.Kebaberize(orig).ShouldBe(dest);

    }
}