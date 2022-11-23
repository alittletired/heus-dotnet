using System.Diagnostics;
using System.Reflection;
using Heus.IntegratedTests;
using Xunit.Sdk;

namespace Heus.Auth.IntegratedTests;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class UseUnitOfWorkAttribute: BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
      if(UnitOfWorkManagerAccessor.UnitOfWorkManager?.Current==null)
        UnitOfWorkManagerAccessor.UnitOfWorkManager?.Begin();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        UnitOfWorkManagerAccessor.UnitOfWorkManager.Current?.CompleteAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        UnitOfWorkManagerAccessor.UnitOfWorkManager.Current?.Dispose();
    }
}
