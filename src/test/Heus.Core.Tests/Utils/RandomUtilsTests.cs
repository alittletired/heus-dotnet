using Heus.Core.Utils;
using System.IO;
namespace Heus.Core.Tests.Utils;
[TestClass]
public class RandomUtils_Tests
{

    [TestMethod]
    [DataRow(0, "num must be a positive integer.input: 0")]
    [DataRow(-1, "num must be a positive integer.input: -1")]
    [DataRow(11, "num must less than 10.input: 11")]
    [DataRow(10, "num must less than 10.input: 10")]
    public void GenerateNumberString_InvalidParameterCheck_Test(int len, string error)
    {
        var exception = Assert.ThrowsException<InvalidDataException>(() => RandomUtils.GenerateNumberString(len));
        exception.Message.ShouldBe(error);

    }
    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    [DataRow(8)]
    [DataRow(9)]
    public void GenerateNumberString_Test(int len)
    {
        RandomUtils.GenerateNumberString(len).Length.ShouldBe(len);

    }
}