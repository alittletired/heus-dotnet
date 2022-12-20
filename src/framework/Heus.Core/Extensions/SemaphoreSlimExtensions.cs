using Heus.Core.Common;

namespace System.Threading;

public static class SemaphoreSlimExtensions
{
    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim)
    {
        await semaphoreSlim.WaitAsync();
        return GetDispose(semaphoreSlim);
    }
    private static IDisposable GetDispose(this SemaphoreSlim semaphoreSlim)
    {
        return  DisposeAction.Create(() =>
        {
            semaphoreSlim.Release();
        });
    }
  
    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
    {
     var result=   await semaphoreSlim.WaitAsync(millisecondsTimeout);
        if (result)
        {
            return GetDispose(semaphoreSlim);
        }
        return NullDisposable.Instance;
    }
}
