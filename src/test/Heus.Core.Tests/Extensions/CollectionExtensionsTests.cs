namespace Heus.Core.Tests.Extensions;
[TestClass]
public class CollectionExtensionsTests
{
    private readonly ICollection<int> _emptyList = new List<int>();
    private readonly ICollection<int> _nullList = null!;
    private ICollection<int> TestList => new List<int>() { 1, 2, 3 };
    [TestMethod]
    public void IsNullOrEmpty_Test()
    {
        _nullList.IsNullOrEmpty().ShouldBe(true);
        _emptyList.IsNullOrEmpty().ShouldBe(true);
        TestList.IsNullOrEmpty().ShouldBe(false);
    }

    [TestMethod]
    public void TryAdd_Test()
    {
        TestList.TryAdd(5).ShouldBe(true);
        TestList.TryAdd(3).ShouldBe(false);
    }

    [TestMethod]
    public void ForEach_Test()
    {
        var list = TestList;
        var sum = 0;
        list.ForEach(i => sum += i);
        list.Sum().ShouldBe(sum);
        _nullList.ForEach(I => { });
    }
}