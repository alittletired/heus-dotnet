namespace Heus.Core.Tests.Extensions;

public class DictionaryExtensionsTests
{
    private Dictionary<int, int> _dict = new() { { 1, 10 }, { 2, 20 } };

    [Theory]
    [InlineData(1, 10)]
    [InlineData(3, 30)]
    public void GetOrAdd_Tests(int key, int value)
    {
        _dict.GetOrAdd(key, k => value).ShouldBe(value);
    }

}