using System.Reflection;
using Heus.Core.Utils;


namespace Heus.Core.Tests.Utils;
[TestClass]
public class TestClass
{
    public Task TestVoidAsync(){return  Task.CompletedTask;}

    public Task<int> TestReturnAsync()
    {
        return Task.FromResult(1);
    }
    public int TestReturnInt()
    {
        return 1;
    }
}
[TestClass]
public class AsyncUtilsTests
{
    private static TypeInfo _testType = typeof(TestClass).GetTypeInfo();

    [TestMethod]
    public void IsAsync()
    {
        _testType.DeclaredMethods.First(s => s.Name == nameof(TestClass.TestVoidAsync)).IsAsync().ShouldBeTrue();
        _testType.DeclaredMethods.First(s => s.Name == nameof(TestClass.TestReturnAsync)).IsAsync().ShouldBeTrue();
    }

    [TestMethod]
    public void UnwrapTask()
    {
        _testType.DeclaredMethods.First(s => s.Name == nameof(TestClass.TestVoidAsync)).ReturnType.UnwrapTask()
            .ShouldBe(typeof(void));
        _testType.DeclaredMethods.First(s => s.Name == nameof(TestClass.TestReturnAsync)).ReturnType.UnwrapTask()
            .ShouldBe(typeof(int));
        
        _testType.DeclaredMethods.First(s => s.Name == nameof(TestClass.TestReturnInt)).ReturnType.UnwrapTask()
            .ShouldBe(typeof(int));
    }
}