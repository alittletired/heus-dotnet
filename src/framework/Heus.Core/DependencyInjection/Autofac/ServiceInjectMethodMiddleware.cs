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
        public PipelinePhase Phase => PipelinePhase.RegistrationPipelineStart;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            next(context);
            if( context.Instance is IInitialization initialization)
            {
                var serviceProvider = context.Resolve<IServiceProvider>();
                initialization.Initialize(serviceProvider);
            }
        }
    }
}
