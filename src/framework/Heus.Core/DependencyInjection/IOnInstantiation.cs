namespace Heus.Core.DependencyInjection;


/// <summary>
/// 方法注入接口,在组件初始化时会被ioc容器调用
/// </summary>
//完全使用构造函数注入时，子类需要传入基类依赖的组件，导致定义较为麻烦.虽然autofac提供了属性注入，但是需要将属性定义成public，这样暴露基类属性并不是好的设计
public interface IInitialization
{
    void Initialize(IServiceProvider serviceProvider);
}
