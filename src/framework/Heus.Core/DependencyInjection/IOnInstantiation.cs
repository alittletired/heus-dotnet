namespace Heus.Core.DependencyInjection;

/// <summary>
/// 方法注入接口，无法通过构造函数注入，不想暴露公共属性注入时使用。
/// </summary>
public interface IOnInstantiation
{
    void OnInstantiation(IServiceProvider serviceProvider);
}
