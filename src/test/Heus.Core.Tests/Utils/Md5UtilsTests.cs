using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class Md5UtilsTests
{
    [Theory]
    [InlineData("1","C4CA4238A0B923820DCC509A6F75849B")]
    public void HashTest(string input,string expect)
    {
        Md5Utils.ToHash(input).ShouldBe(expect);

    }
}