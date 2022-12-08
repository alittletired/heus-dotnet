using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class TestTypeUtilsClass
{
    public static Func<int> Func1 { get; }= () => 1;
    public static Func<int,int> Func2 { get; }= (int i) => i;
    public static void Action1() { }
}
record TestRecord(int Prop1)
{
    
}

public class TypeUtilsTests
{
    [Theory]
    [InlineData(typeof(TestRecord), true)]
    [InlineData(typeof(TypeUtilsTests), false)]
    public void IsRecordType(Type type, bool isRecord)
    {
        TypeUtils.IsRecordType(type).ShouldBe(isRecord);
    }

    [Fact]
    public void IsFunc()
    {
        TypeUtils.IsFunc(TestTypeUtilsClass.Func1).ShouldBe(true);
        TypeUtils.IsFunc(TestTypeUtilsClass.Func2).ShouldBe(true);
        TypeUtils.IsFunc(TestTypeUtilsClass.Action1).ShouldBe(false);
        TypeUtils.IsFunc(null).ShouldBe(false);
        TypeUtils.IsFunc<Func<int>>(TestTypeUtilsClass.Func1).ShouldBe(true);
        TypeUtils.IsFunc<Func<int,int>>(TestTypeUtilsClass.Func1).ShouldBe(false);
        TypeUtils.IsFunc<Func<int,int>>(TestTypeUtilsClass.Func2).ShouldBe(true);
      

    }
}