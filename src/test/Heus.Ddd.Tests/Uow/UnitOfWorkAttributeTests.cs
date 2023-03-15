using System.Reflection;
using Heus.Core;
using Heus.Ddd.Uow;
using Heus.TestBase;

namespace Heus.Ddd.Tests.Uow;

[UnitOfWork]
public class UnitOfWorkTest1
{
}

public class UnitOfWorkTest2
{
    [UnitOfWork]
    public void WithUow() { }
    public void WithOutUow() { }
}


public class UnitOfWorkAttributeTests : IntegratedTestBase<CoreModuleInitializer>
{
    [Theory]
    [InlineData(typeof(UnitOfWorkTest1), true)]
    [InlineData(typeof(UnitOfWorkTest2), true)]
    [InlineData(typeof(UnitOfWorkAttributeTests), false)]
    public void Test_IsUnitOfWorkType(Type type, bool isUnitOfWorkType)
    {
        UnitOfWorkHelper.IsUnitOfWorkType(type.GetTypeInfo()).ShouldBe(isUnitOfWorkType);
    }

    [Theory]
    [InlineData(typeof(UnitOfWorkTest2), nameof(UnitOfWorkTest2.WithUow), false)]
    [InlineData(typeof(UnitOfWorkTest2), nameof(UnitOfWorkTest2.WithOutUow), true)]
    [InlineData(typeof(UnitOfWorkTest2), "NotExists", true)]
    public void Test_GetUnitOfWorkAttributeOrNull(Type type, string methodName, bool isnull)
    {
        var methods = type.GetTypeInfo().DeclaredMethods.FirstOrDefault(s => s.Name == methodName);

        var attribute = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methods);
        (attribute == null).ShouldBe(isnull);
    }
}