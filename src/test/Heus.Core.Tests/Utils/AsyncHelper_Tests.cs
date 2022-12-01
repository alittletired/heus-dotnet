using System.Reflection;
using Heus.Core.Utils;
using Shouldly;
using Xunit;

namespace Heus.Core.Tests.Utils;

public class TestClass
{
    public Task TestVoidAsync(){return  Task.CompletedTask;}

    public Task<int> TestReturnAsync()
    {
        return Task.FromResult(1);
    }
}

public class AsyncHelper_Tests
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
    }
}