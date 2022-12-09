namespace Heus.Core.Tests.Extensions;

public class ListExtensionsTests
{
    private List<string> _data = new() {
        "1",
        "2",
        "3",
        "4",
        "5"
    };


    [Theory]
    [InlineData("a")]
    public void AddFirst_Test(string value)
    {
        var data = new List<string>(_data);
        data.AddFirst(value);
        data[0].ShouldBe(value);
    }
    [Theory]
    [InlineData("z")]
    public void AddLast_Test(string value)
    {
        var data = new List<string>(_data);
        data.AddLast(value);
        data[^1].ShouldBe(value);
    }
  
}