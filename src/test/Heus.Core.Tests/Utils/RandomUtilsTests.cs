using Heus.Core.Utils;
using System.IO;
namespace Heus.Core.Tests.Utils;

public class RandomUtils_Tests
{

    [Theory]
    [InlineData(0, "num must be a positive integer.input: 0")]
    [InlineData(-1, "num must be a positive integer.input: -1")]
    [InlineData(11, "num must less than 10.input: 11")]
    [InlineData(10, "num must less than 10.input: 10")]
    public void GenerateNumberString_InvalidParameterCheck_Test(int len, string error)
    {
        var exception = Assert.Throws<InvalidDataException>(() => RandomUtils.GenerateNumberString(len));
        exception.Message.ShouldBe(error);

    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void GenerateNumberString_Test(int len)
    {
        RandomUtils.GenerateNumberString(len).Length.ShouldBe(len);

    }
}