namespace Heus.Core.Utils;
public static class AsyncLocalUtils
{
    public static IDisposable BeginScope<T>(AsyncLocal<T?> asyncLocal, T? newValue)
    {
        var parent = asyncLocal.Value;
        asyncLocal.Value = newValue;
        return DisposeAction.Create(() =>
        {
            asyncLocal.Value = parent;
        });
    }
}
