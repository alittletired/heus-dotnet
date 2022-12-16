using Heus.Core.Common;

namespace Heus.Core.Tests.Common;

public class TestEnumClass: EnumClass<TestEnumClass>
{
    private TestEnumClass(string name, int value, string title) : base(name, value, title)
    {
    }
    public readonly static TestEnumClass Normal = new(nameof(Normal),  0, "正常");
    public readonly static TestEnumClass Disabled = new(nameof(Disabled), 1, "禁用");
}
[TestClass]
public class EnumClassTests
{
    [TestMethod]
    public void GetEnumOptions_Test()
    {
        TestEnumClass.GetEnumOptions().Length.ShouldBe(2);
       

    }

    [TestMethod]
    public void FromName_Test()
    {
      var disabled=  TestEnumClass.FromName(nameof(TestEnumClass.Disabled));
      disabled .ShouldBe(TestEnumClass.Disabled);
      
      var normal=  TestEnumClass.FromName(nameof(TestEnumClass.Normal));
      normal .ShouldBe(TestEnumClass.Normal);
      
      (disabled==normal).ShouldBeFalse();
      disabled.ToString().ShouldBe(nameof(TestEnumClass.Disabled));
      (disabled==1).ShouldBeTrue();
      ((TestEnumClass)1).ShouldBe(disabled);
      TestEnumClass.Disabled.ShouldBeGreaterThan(TestEnumClass.Normal);
      
    }
    [TestMethod]
    public void TryFromName_Test()
    {
        TestEnumClass.TryFromName(nameof(TestEnumClass.Disabled),out var disabled).ShouldBeTrue();
        disabled.ShouldBe(TestEnumClass.Disabled);
        TestEnumClass.TryFromName("notexists",out var notexists).ShouldBeFalse();
        notexists.ShouldBeNull();
    }

    [TestMethod]
    public void TryFromValue_Test()
    {
        TestEnumClass.TryFromValue(1,out var disabled).ShouldBeTrue();
        disabled.ShouldBe(TestEnumClass.Disabled);
    }

    [TestMethod]
    public void EqualsTest()
    {
        TestEnumClass.Disabled.GetHashCode().ShouldBe(TestEnumClass.Disabled.Value.GetHashCode());
        TestEnumClass.Disabled.Title.ShouldNotBeNull();
        TestEnumClass.Disabled.Equals(30).ShouldBeFalse();
        TestEnumClass.Disabled.Equals(null).ShouldBeFalse();
        TestEnumClass.Disabled?.Equals( TestEnumClass.Normal).ShouldBeFalse();
        TestEnumClass.Disabled?.Equals( TestEnumClass.Disabled).ShouldBeTrue();
      
        (TestEnumClass.Disabled!>TestEnumClass.Normal!).ShouldBeTrue();
        (TestEnumClass.Disabled!<TestEnumClass.Normal).ShouldBeFalse();
        (TestEnumClass.Disabled!>=TestEnumClass.Normal).ShouldBeTrue();
        (TestEnumClass.Disabled!<=TestEnumClass.Normal).ShouldBeFalse();
    }

}