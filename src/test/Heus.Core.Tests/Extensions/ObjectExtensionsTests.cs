namespace Heus.Core.Tests.Extensions;
[TestClass]
public class ObjectExtensionsTests
{
    [TestMethod]
    public void ConvertToTest()
    {
        var id = Guid.NewGuid();
        id.ToString().ConvertTo<Guid>().ShouldBe(id);
        var b = "10";
        b.ConvertTo<int>().ShouldBe(10);
    }
}