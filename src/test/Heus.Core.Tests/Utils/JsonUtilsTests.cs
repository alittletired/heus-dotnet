using Heus.Core.Common;
using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class TestJsonEnum : EnumClass<TestJsonEnum>
{
    public TestJsonEnum(string name, int value, string title) : base(name, value, title)
    {
    }
    public static TestJsonEnum  Test0=  new(nameof(Test0), 0, "Test12");
    public static TestJsonEnum  Test1=  new(nameof(Test1), 1, "Test1");
}
public class TestJson
{
    public required long Id { get; init; }
    public required TestJsonEnum Test1{ get; init; }
}
public class JsonUtilsTests
{
    private const string _arrJson = """
[1,2,3]
""";
    private const string _json1 = """
{"id":123,"test1":1}
""";
    private const string _json2 = """
{"id":"123","test1":"Test1"}
""";
    [Fact]
    public void Deserialize_Array_Tests()
    {

        JsonUtils.Deserialize<List<int>>(_arrJson).ShouldNotBeEmpty();
        JsonUtils.Deserialize<List<int>>("").ShouldBeNull();
    }
    [Fact]
    public void Deserialize_Enum_Number_Convert()
    {
      
        var obj= JsonUtils.Deserialize<TestJson>(_json1);
        obj.ShouldNotBeNull();
       
        obj.Test1.ShouldBe(TestJsonEnum.Test1);
        JsonUtils.Serialize(obj).ShouldNotBeEmpty();
    }
    [Fact]
    public void Deserialize_Enum_String_Convert()
    {
      
       var obj= JsonUtils.Deserialize<TestJson>(_json2);
       obj.ShouldNotBeNull();
       obj.Test1.ShouldBe(TestJsonEnum.Test1);
       JsonUtils.Serialize(obj).ShouldNotBeEmpty();
    }
    
    [Fact]
    public void Deserialize_Long_To_Long_Convert()
    {
      
        var obj= JsonUtils.Deserialize<TestJson>(_json1);
        obj.ShouldNotBeNull();
        obj.Id.ShouldBe(123);
        
    }
    [Fact]
    public void Deserialize_Long_To_String_Convert()
    {
        var obj2= JsonUtils.Deserialize<TestJson>(_json2);
        obj2.ShouldNotBeNull();
        obj2.Id.ShouldBe(123);
    }
}