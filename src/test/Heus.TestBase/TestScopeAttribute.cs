
using System.Reflection;
using Xunit.Sdk;

namespace Heus.TestBase;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal class TestScopeAttribute: BeforeAfterTestAttribute
{

    public override void Before(MethodInfo methodUnderTest)
    {
        UnitOfWorkManagerAccessor.UnitOfWorkManager.Begin();
    }
       
    public override void After(MethodInfo methodUnderTest)
    {
        var uow = UnitOfWorkManagerAccessor.UnitOfWorkManager.Current;
        if (uow != null)
        {
            uow.CompleteAsync();
            uow.Dispose();
        }
    }
}
