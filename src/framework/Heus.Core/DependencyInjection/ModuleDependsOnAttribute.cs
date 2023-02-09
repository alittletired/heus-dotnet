namespace Heus.Core.DependencyInjection;

/// <summary>
/// 依赖属性，主要用于模块加载，如业务模块需要依赖平台的模块则使用
/// [DependsOn(typeof(BusinessServiceModule))]
///public class DataServiceModule : IServiceModule{}
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ModuleDependsOnAttribute<T> : Attribute where T:IModuleInitializer
{
    

}

