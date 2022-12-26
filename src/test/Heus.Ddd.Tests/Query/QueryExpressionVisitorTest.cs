namespace Heus.Ddd.Tests.Query;

public class QueryData
{
    public int Id { get; set; }
    public string Name { get; set; }

    public QueryData(int id)
    {
        Id = id;
        Name = "name" + id;
    }
}
public class QueryExpressionVisitorTest
{
    private static IEnumerable<QueryData> TestDatas = Enumerable.Range(1, 10).Select(i=>new QueryData(i));

    public void TestEquals()
    {
        
        
    }
}