using System.Collections;
using System.Reflection;
using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class TestTypeUtilsClass
{
    public static Func<int> Func1 { get; }= () => 1;
    public static Func<int,int> Func2 { get; }= (int i) => i;
    public static void Action1() { }
    public static TestTypeUtilsClass Instance { get; set; } = null!;
    public static TestTypeUtilsClass? Instance1 { get; set; } = null!;
    public static int? NullableInt { get; set; }
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
    public void IsNullable_Property()
    {
        var props = typeof(TestTypeUtilsClass).GetTypeInfo().DeclaredProperties;
        TypeUtils.IsNullable(props.First(s=>s.Name==nameof(TestTypeUtilsClass.Instance))).ShouldBe(false);
        TypeUtils.IsNullable(props.First(s=>s.Name==nameof(TestTypeUtilsClass.Instance1))).ShouldBe(true);
       
        
    }
    [Fact]
    public void IsNullable_Type()
    {
        var props = typeof(TestTypeUtilsClass).GetTypeInfo().DeclaredProperties;
        TypeUtils.IsNullable(props.First(s=>s.Name==nameof(TestTypeUtilsClass.Instance)).PropertyType).ShouldBe(false);
        TypeUtils.IsNullable(props.First(s=>s.Name==nameof(TestTypeUtilsClass.Instance1)).PropertyType).ShouldBe(false);
        TypeUtils.IsNullable(props.First(s=>s.Name==nameof(TestTypeUtilsClass.NullableInt)).PropertyType).ShouldBe(true);
    }

    [Theory]
    [InlineData(typeof(List<string>), typeof(string))]
    [InlineData(typeof(ArrayList), typeof(object))]
    [InlineData(typeof(object), null)]
    public void TryGetEnumerableType_Test(Type type, Type? itemType)
    {
        TypeUtils.GetEnumerableItemType(type).ShouldBe(itemType);
    }
    [Theory]
    [InlineData(typeof(List<string>), null)]
    [InlineData(typeof(int), 0)]
    [InlineData(typeof(object), null)]
    public void GetDefaultValue_Test(Type type, object? value)
    {
        TypeUtils.GetDefaultValue(type).ShouldBe(value);
    }
    [Fact]
    public void IsDefaultValue_Test()
    {
        var a = 2;
        var b=0;
        TypeUtils.IsDefaultValue(a).ShouldBe(false);
        TypeUtils.IsDefaultValue(b).ShouldBe(true);
        TypeUtils.IsDefaultValue(TestTypeUtilsClass.Instance1).ShouldBe(true);
        TypeUtils.IsDefaultValue(TestTypeUtilsClass.Instance).ShouldBe(true);
        
    }
    [Theory]
    [InlineData(typeof(List<string>), "List<string>")]
    [InlineData(typeof(int), "number")]
    [InlineData(typeof(bool), "boolean")]
    [InlineData(typeof(Guid), "string")]
    [InlineData(typeof(int?), "number?")]
    public void GetSimplifiedName_Test(Type type, string value)
    {
        TypeUtils.GetSimplifiedName(type).ShouldBe(value);
    }
    [Fact]
    public void ConvertFromString_Test()
    {
        TypeUtils.ConvertFromString<bool>("True").ShouldBe(true);

    }
    [Fact]
    public void ConvertFrom_Test()
    {
        TypeUtils.ConvertFrom<bool>("True").ShouldBe(true);

    }
    
}