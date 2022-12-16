using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;
[TestClass]
public class JsonUtilsTests
{
    private const string _arrJson = """
[1,2,3]
""";
    [TestMethod]
    public void Deserialize_Tests()
    {

        JsonUtils.Deserialize<List<int>>(_arrJson).ShouldNotBeEmpty();
        JsonUtils.Deserialize<List<int>>("").ShouldBeNull();
    }
}