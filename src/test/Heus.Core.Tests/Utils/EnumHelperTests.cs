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
public class EnumHelperTests
{
    [Theory]
    [InlineData(TestEnum.One,"value1")]
    [InlineData(TestEnum.Two,"value2")]
    [InlineData(TestEnum.Three,"Three")]
    public void GetSummary(TestEnum enumValue,string text)
    {
        EnumHelper.GetSummary(enumValue).ShouldBe(text);
        
    }
}