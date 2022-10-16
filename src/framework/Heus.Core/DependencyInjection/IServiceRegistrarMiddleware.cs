namespace Heus.Core.DependencyInjection;

// internal class MiddlewareChain
// {
//     public void Next();
// }

public class RegisterContext
{
    public RegisterContext(Type type, IServiceCollection services)
    {
        ImplementationType = type;
        Services = services;
    }


    public IServiceCollection Services { get; }
    public  Type ImplementationType { get; }
}
public interface IServiceRegistrarMiddleware
{
    void OnRegister(RegisterContext context, Action next);
}