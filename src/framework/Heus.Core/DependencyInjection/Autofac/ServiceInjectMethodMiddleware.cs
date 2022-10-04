using Autofac.Core.Resolving.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Heus.Core.DependencyInjection.Autofac
{
    internal class ServiceInjectMethodMiddleware : IResolveMiddleware
    {
        public PipelinePhase Phase => PipelinePhase.ServicePipelineEnd;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            next(context);
            if( context.Instance is IInjectServiceProvider initialization)
            {
                var serviceProvider = context.Resolve<IServiceProvider>();
                initialization.ServiceProvider=serviceProvider;
            }
        }
    }
}
