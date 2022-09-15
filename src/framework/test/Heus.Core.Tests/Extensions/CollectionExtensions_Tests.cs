
namespace Heus.Core.Tests.Extensions;

public class CollectionExtensions_Tests
{
    [Fact]
    public void IsNullOrEmpty_Test()
    {
        var collection = new List<int> { 4, 5, 6 };
        collection.IsNullOrEmpty().ShouldBeFalse();
        var emptyList = new List<int>();
        emptyList.IsNullOrEmpty().ShouldBeTrue();
        List<int>? nullList = null;
        nullList.IsNullOrEmpty().ShouldBeTrue();

    }
    [Fact]
    public void AddIfNotContains_Test()
    {
        var collection = new List<int> { 4, 5, 6 };

        collection.TryAdd(5);
        collection.Count.ShouldBe(3);

        collection.TryAdd(42);
        collection.Count.ShouldBe(4);

    }

}

