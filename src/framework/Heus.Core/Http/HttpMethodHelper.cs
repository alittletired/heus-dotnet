using System.Reflection;

namespace Heus.Core.Http;

public static class HttpMethodHelper
{
    public readonly static HttpMethod DefaultHttpVerb = HttpMethod.Post;

    public static Dictionary<HttpMethod, string[]> ConventionalPrefixes { get;  } = new ()
    {
        {HttpMethod.Get, new[] {"GetList", "GetAll", "Get"}},
        {HttpMethod.Put , new[] {"Put", "Update"}},
        {HttpMethod.Delete, new[] {"Delete", "Remove"}},
        {HttpMethod.Post , new[] {"Create", "Add", "Insert", "Post"}},
        {HttpMethod.Patch , new[] {"Patch"}}
    };
    public static HttpMethod GetHttpMethod(MethodInfo method)
    {
        return GetHttpMethod(method.Name);
    }
    public static HttpMethod GetHttpMethod(string methodName)
    {
        foreach (var conventionalPrefix in ConventionalPrefixes)
        {
            if (conventionalPrefix.Value.Any(prefix => methodName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            {
                return conventionalPrefix.Key;
            }
        }

        return DefaultHttpVerb;
    }


    
   
}
