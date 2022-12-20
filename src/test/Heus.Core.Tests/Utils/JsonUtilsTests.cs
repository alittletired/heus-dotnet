using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class JsonUtilsTests
{
    private const string _arrJson = """
[1,2,3]
""";
    [Fact]
    public void Deserialize_Tests()
    {

        JsonUtils.Deserialize<List<int>>(_arrJson).ShouldNotBeEmpty();
        JsonUtils.Deserialize<List<int>>("").ShouldBeNull();
    }
}