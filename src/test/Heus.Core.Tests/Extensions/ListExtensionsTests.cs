namespace Heus.Core.Tests.Extensions;
[TestClass]
public class ListExtensionsTests
{
    private List<string> _data = new() {
        "1",
        "2",
        "3",
        "4",
        "5"
    };


    [TestMethod]
    [DataRow("a")]
    public void AddFirst_Test(string value)
    {
        var data = new List<string>(_data);
        data.AddFirst(value);
        data[0].ShouldBe(value);
    }
    [TestMethod]
    [DataRow("z")]
    public void AddLast_Test(string value)
    {
        var data = new List<string>(_data);
        data.AddLast(value);
        data[^1].ShouldBe(value);
    }

}