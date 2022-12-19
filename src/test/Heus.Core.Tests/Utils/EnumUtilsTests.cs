using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public enum TestEnum
{
    /// <summary>
    /// value1
    /// </summary>
    One,
    /// <summary>
    /// value2
    /// </summary>
    Two,
    Three
}
[TestClass]
public class EnumHelperTests
{
    [TestMethod]
    [DataRow(TestEnum.One, "value1")]
    [DataRow(TestEnum.Two, "value2")]
    [DataRow(TestEnum.Three, "Three")]
    public void GetSummary(TestEnum enumValue, string text)
    {
        EnumUtils.GetSummary(enumValue).ShouldBe(text);

    }
}