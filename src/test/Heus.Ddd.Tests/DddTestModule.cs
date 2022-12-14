
using Heus.Core.DependencyInjection;
using Heus.TestBase;

namespace Heus.Ddd.Tests;
[DependsOn(typeof (TestBaseModule))]
internal class DddTestModule :ModuleInitializerBase
{
}
