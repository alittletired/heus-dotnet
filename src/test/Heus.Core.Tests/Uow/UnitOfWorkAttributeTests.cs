using System.Reflection;
using Heus.Core.Uow;
using Heus.TestBase;

namespace Heus.Core.Tests.Uow;

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

[TestClass]
public class UnitOfWorkAttributeTests : IntegratedTestBase<CoreModuleInitializer>
{
    [TestMethod]
    [DataRow(typeof(UnitOfWorkTest1), true)]
    [DataRow(typeof(UnitOfWorkTest2), true)]
    [DataRow(typeof(UnitOfWorkAttributeTests), false)]
    public void Test_IsUnitOfWorkType(Type type, bool isUnitOfWorkType)
    {
        UnitOfWorkHelper.IsUnitOfWorkType(type.GetTypeInfo()).ShouldBe(isUnitOfWorkType);
    }

    [TestMethod]
    [DataRow(typeof(UnitOfWorkTest2), nameof(UnitOfWorkTest2.WithUow), false)]
    [DataRow(typeof(UnitOfWorkTest2), nameof(UnitOfWorkTest2.WithOutUow), true)]
    [DataRow(typeof(UnitOfWorkTest2), "NotExists", true)]
    public void Test_GetUnitOfWorkAttributeOrNull(Type type, string methodName, bool isnull)
    {
        var methods = type.GetTypeInfo().DeclaredMethods.FirstOrDefault(s => s.Name == methodName);

        var attribute = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methods);
        (attribute == null).ShouldBe(isnull);
    }
}