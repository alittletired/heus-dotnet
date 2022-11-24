namespace Heus.Core.DependencyInjection;

/// <summary>
/// 自动注入IServiceProvider，此接口方便超类能够拿到IServiceProvider，可以构造较为复杂的属性
/// </summary>
public interface IInjectServiceProvider
{
    void SetServiceProvider(IServiceProvider serviceProvider);
}