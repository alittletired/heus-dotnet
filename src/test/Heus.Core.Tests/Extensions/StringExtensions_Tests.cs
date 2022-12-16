using System.Text;

namespace Heus.Core.Tests.Extensions;
[TestClass]
public class StringExtensions_Tests
{
    [TestMethod]
    [DataRow("a", true)]
    [DataRow("", false)]
    [DataRow(" ", true)]
    [DataRow(null, false)]
    public void HasText_Test(string? obj, bool expect)
    {
        obj.HasText().ShouldBe(expect);
    }

    [TestMethod]
    [DataRow("a", false)]
    [DataRow("", true)]
    [DataRow(" ", false)]
    [DataRow(null, true)]
    public void IsNullOrEmpty_Test(string? obj, bool expect)
    {
        obj.IsNullOrEmpty().ShouldBe(expect);
    }

    [TestMethod]
    [DataRow("a", false)]
    [DataRow("", true)]
    [DataRow(" ", true)]
    [DataRow(null, true)]
    public void IsNullOrWhiteSpace_Test(string? obj, bool expect)
    {
        obj.IsNullOrWhiteSpace().ShouldBe(expect);
    }

    [TestMethod]

    public void NormalizeLineEndingsTest()
    {
        var obj = "\r\na\rb";
        StringExtensions.NormalizeLineEndings(obj).ShouldBe($"{Environment.NewLine}a{Environment.NewLine}b");
    }

    [TestMethod]
    [DataRow("a")]
    public void GetBytesTest(string str)
    {
        str.GetBytes().ShouldNotBeNull();
        str.GetBytes(Encoding.UTF8).ShouldNotBeNull();
    }
    [TestMethod]
    public void JoinAsStringTest()
    {
        IEnumerable<string> str = new List<string>() { "a", "b", "c" };
        str.JoinAsString(",").ShouldBe("a,b,c");
    }
}