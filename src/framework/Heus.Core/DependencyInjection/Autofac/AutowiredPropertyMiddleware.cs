using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;

namespace Heus.Core.DependencyInjection.Autofac;
internal class AutowiredPropertyMiddleware : IResolveMiddleware
{
    public PipelinePhase Phase => PipelinePhase.Activation;

    public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
    {
        next(context);
        if (!context.NewInstanceActivated)
        {
            return;
        }
        var instance = context.Instance!;
        var selector = PropertiesAutowiredHelper.GetSelector(instance.GetType());
        selector.Autowired(context);
        
    }
}
