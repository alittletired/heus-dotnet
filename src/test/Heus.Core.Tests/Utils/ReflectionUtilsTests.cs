using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class Class1
{
    public class Class3
    {
        public string E { get; } = "e";
    }
    public class Class2
    {
        public int C { get; } = 4;
        public Class3 D { get; } = new Class3();
    }
    public int A { get; } = 1;
    public Class2 B { get; } = new Class2();
}
public class ReflectionUtilsTests
{
    private readonly object _obj = new Class1();

    [Theory]
    [InlineData("A", 1)]
    [InlineData("B.C", 4)]
    [InlineData("B.D.E", "e")]
    public void GetValueByPathTests(string path, object value)
    {
        ReflectionUtils.GetValueByPath(_obj, path).ShouldBe(value);
    }
    
}