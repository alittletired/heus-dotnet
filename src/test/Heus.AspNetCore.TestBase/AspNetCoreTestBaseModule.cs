using Heus.Core.DependencyInjection;
using Heus.TestBase;

namespace Heus.AspNetCore.TestBase;
[DependsOn(typeof(TestBaseModule))]
public class AspNetCoreTestBaseModule: ModuleInitializerBase
{
}
