namespace Heus.Core.Tests.Extensions;

public class QueryableExtensionsTest
{
    [Fact]
    public void WhereIfTest()
    {
        List<int> data = new() {
            1,
            2,
            3
           
        };
        var query = data.AsQueryable();
        var data1= query.WhereIf(true, i => i > 1).ToList();
        data1.Count.ShouldNotBe(data.Count);
        var data2=query.WhereIf(false, i => i > 1).ToList();
        data2.Count.ShouldBe(data.Count);

    }
}