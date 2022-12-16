using System.Diagnostics;

namespace Heus.Core.Tests.Threading;
[TestClass]
public class SemaphoreSlimExtensionsTests
{
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    private Func<Task> _action = async () =>
    {
        using var scope = await _semaphore.LockAsync();
        await Task.Delay(500);


    };

    private Func<Task> _action2 = async () =>
    {
        using var scope = await _semaphore.LockAsync();
        await Task.Delay(1500);


    };

    private Func<Task> _action3 = async () =>
    {
        using var scope = await _semaphore.LockAsync(500);
        await Task.Delay(500);


    };

    [TestMethod]
    public void LockAsync_Test()
    {
        var watch = Stopwatch.StartNew();
        Task.Run(_action);
        Task.Run(_action);

    }

    [TestMethod]
    public void LockAsync_Test1()
    {
        Task.Run(_action);
        Task.Run(_action);

    }
}