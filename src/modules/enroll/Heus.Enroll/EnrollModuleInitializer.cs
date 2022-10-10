
using Heus.Auth;
using Heus.Core.DependencyInjection;

namespace Heus.Business;
[DependsOn(typeof(AuthModuleInitializer))]
public class EnrollModuleInitializer : ModuleInitializerBase
{
   
}