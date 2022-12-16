namespace Heus.Core.Tests.Extensions;
[TestClass]
public class DictionaryExtensionsTests
{
    private Dictionary<int, int> _dict = new() { { 1, 10 }, { 2, 20 } };

    [TestMethod]
    [DataRow(1, 10)]
    [DataRow(3, 30)]
    public void GetOrAdd_Tests(int key, int value)
    {
        _dict.GetOrAdd(key, k => value).ShouldBe(value);
    }

}