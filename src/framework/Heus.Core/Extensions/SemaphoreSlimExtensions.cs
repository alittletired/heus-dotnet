﻿namespace System.Threading;

public static class SemaphoreSlimExtensions
{
    public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim)
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


    public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
    {
        await semaphoreSlim.WaitAsync(timeout);
        return GetDispose(semaphoreSlim);
    }
    public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
    {
        await semaphoreSlim.WaitAsync(millisecondsTimeout);
        return GetDispose(semaphoreSlim);
    }
}
