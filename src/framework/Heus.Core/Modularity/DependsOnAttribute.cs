namespace Heus.Core.Modularity;

/// <summary>
/// 依赖属性，主要用于模块加载，如业务模块需要依赖平台的模块则使用
/// [DependsOn(typeof(BusinessServiceModule))]
///public class DataServiceModule : IServiceModule{}
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute : Attribute
{
    /// <summary>
    /// 依赖的类型
    /// </summary>
    public Type[] DependedTypes { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dependedTypes"></param>
    public DependsOnAttribute(params Type[] dependedTypes)
    {
        DependedTypes = dependedTypes;
    }

}

