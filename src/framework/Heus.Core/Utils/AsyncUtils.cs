using System.Reflection;

namespace Heus.Core.Utils;

/***
 * 
 * https://kubernetes.io/docs/tutorials/services/connect-applications-service/#environment-variables
 * KUBERNETES_SERVICE_HOST
 * /.dockerinit
 * if File.exists?('/.dockerenv')
  puts "I'm running in a docker container"
end

if File.exists?('/var/run/secrets/kubernetes.io')
  puts "I'm also running in a Kubernetes pod"
end
*/
/// <summary>
/// Provides some helper methods to work with async methods.
/// </summary>
public static class AsyncUtils
{
    /// <summary>
    /// Checks if given method is an async method.
    /// </summary>
    /// <param name="method">A method to check</param>
    public static bool IsAsync(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        return method.ReturnType.IsTaskOrTaskOfT();
    }

    private static bool IsTaskOrTaskOfT(this Type type)
    {
        return type == typeof(Task) ||
               (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>));
    }

    private static bool IsTaskOfT(this Type type)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
    }

    /// <summary>
    /// Returns void if given type is Task.
    /// Return T, if given type is Task{T}.
    /// Returns given type otherwise.
    /// </summary>
    public static Type UnwrapTask(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (type == typeof(Task))
        {
            return typeof(void);
        }

        return !type.IsTaskOfT() ? type : type.GenericTypeArguments[0];
    }


}