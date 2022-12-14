using System.Reflection;
using Heus.Core.Utils;


namespace Heus.Core.Tests.Utils;

public class TestClass
{
    public Task TestVoidAsync() { return Task.CompletedTask; }

    public Task<int> TestReturnAsync()
    {
        return Task.FromResult(1);
    }
    public int TestReturnInt()
    {
        return 1;
    }
}

public class AsyncUtilsTests
{
    private static TypeInfo _testType = typeof(TestClass).GetTypeInfo();

    [Fact]
    public void IsAsync()
    {
        _testType.DeclaredMethods.First(s => s.Name == nameof(TestClass.TestVoidAsync)).IsAsync().ShouldBeTrue();
        _testType.DeclaredMethods.First(s => s.Name == nameof(TestClass.TestReturnAsync)).IsAsync().ShouldBeTrue();
    }

    [Fact]
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