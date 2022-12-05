using Heus.Core.DependencyInjection;

namespace Heus.AspNetCore;

internal interface IApplicationBuilderAccessor
{
    IApplicationBuilder ApplicationBuilder { get; set; }
}
//aspnet 没有注册IApplicationBuilder，而其他地方需要使用，因此定义该类
internal class ApplicationBuilderAccessor :IApplicationBuilderAccessor, ISingletonDependency
{
    public IApplicationBuilder ApplicationBuilder { get; set; } = null!;
}