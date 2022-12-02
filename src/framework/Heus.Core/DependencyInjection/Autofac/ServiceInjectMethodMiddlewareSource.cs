using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;


namespace Heus.Core.DependencyInjection.Autofac;

internal class ServiceInjectMethodMiddlewareSource : IServiceMiddlewareSource
{
    private readonly IResolveMiddleware _middleware=new ServiceInjectMethodMiddleware();
    public void ProvideMiddleware(Service service, IComponentRegistryServices availableServices, IResolvePipelineBuilder pipelineBuilder)
    {
        
        
        if (service is TypedService typedService)
        {
            // && typedService.ServiceType.IsAssignableTo<IInjectServiceProvider>()
           if( typedService.Description.Contains("Heus"))
            {
                pipelineBuilder.Use(_middleware);     
            }
           
        }
    }
}
