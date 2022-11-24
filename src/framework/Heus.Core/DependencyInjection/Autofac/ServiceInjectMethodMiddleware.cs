using Autofac.Core.Resolving.Pipeline;
using Autofac;

namespace Heus.Core.DependencyInjection.Autofac;

internal class ServiceInjectMethodMiddleware : IResolveMiddleware
{
    public PipelinePhase Phase => PipelinePhase.ServicePipelineEnd;

    public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
    {
        next(context);
        if( context.Instance is IInjectServiceProvider initialization)
        {
            var serviceProvider = context.Resolve<IServiceProvider>();
            initialization.SetServiceProvider(serviceProvider);
        }
    }
}
