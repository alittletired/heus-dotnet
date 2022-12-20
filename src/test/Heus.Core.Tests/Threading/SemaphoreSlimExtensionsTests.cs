using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Heus.Core.Tests.Threading;
public class LockValue
{
    public volatile int Value;

    public class SemaphoreSlimExtensionsTests
    {
        private async Task DelayAction(int delay, SemaphoreSlim semaphore, LockValue value)
        {
            using var scope = await semaphore.LockAsync();
            await Task.Delay(delay);
            Interlocked.Increment(ref value.Value);
        }
        private async Task TimoutAction(int delay, SemaphoreSlim semaphore, LockValue value,int timeout)
        {
            using var scope = await semaphore.LockAsync(timeout);
            await Task.Delay(delay);
            Interlocked.Increment(ref value.Value);
        }

        [Theory]
        [InlineData(50, 4)]
        [InlineData(50, 3)]
        [InlineData(50, 6)]

        public async Task LockAsync_WaitAll(int delay, int repeat)
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var value = new LockValue { Value = 0 };
            var tasks = Enumerable.Repeat(value, repeat).Select(s => DelayAction(delay, semaphore, value));
            await Task.WhenAll(tasks);
            value.Value.ShouldBe(repeat);

        }

        [Theory]
        [InlineData(50,4)]
        [InlineData(50, 5)]
        public async Task LockAsync_WaitAll_Expired(int delay, int repeat)
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var value = new LockValue { Value = 0 };
            var tasks = Enumerable.Repeat(value, repeat).Select(s => DelayAction(delay, semaphore, value));
            var expired = delay * (repeat / 2);
            await Task.WhenAny(Task.WhenAll(tasks), Task.Delay(expired));
            value.Value.ShouldBeLessThan(repeat);
         

        }

        [Theory]
        [InlineData(50, 10, 4)]
        [InlineData(50, 10, 5)]
        public async Task LockAsync_WithTimeout(int delay, int timeout, int repeat)
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var value = new LockValue { Value = 0 };
            var tasks = Enumerable.Repeat(value, repeat).Select(s => TimoutAction(delay, semaphore, value, timeout));
            await Task.WhenAll(tasks);
            value.Value.ShouldBe(repeat);
        }


        [Theory]
        [InlineData(50,10,4)]
        [InlineData(50, 10, 5)]
        public async Task LockAsync_WithTimeout_Expired(int delay,int timeout,int repeat)
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            var value = new LockValue { Value = 0 };
            var tasks = Enumerable.Repeat(value, repeat).Select(s => TimoutAction(delay, semaphore, value, timeout));
            var expired = delay*(repeat/2);
            await Task.WhenAny(Task.WhenAll(tasks), Task.Delay(expired));
         value.Value.ShouldBe(repeat); 
        }
    }
}