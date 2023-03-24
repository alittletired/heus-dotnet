using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heus.Core.DependencyInjection;
using Heus.Ddd;

namespace Heus.Core.Tests;
[ModuleDependsOn<DddModuleInitializer>]
public class TestCoreModuleInitializer: ModuleInitializerBase
{
}
