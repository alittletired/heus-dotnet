using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Heus.Core.Tests.Threading;
public class LockValue
{
    public int Value { get; set; }
    [TestClass]
    public class SemaphoreSlimExtensionsTests
    {

        private Func<SemaphoreSlim, LockValue, Task> _action = async (semaphore, value) =>
        {
            using var scope =await semaphore.LockAsync();
            await Task.Delay(100);
            value.Value++;

        };
        private Func<SemaphoreSlim, LockValue, Task> _action2 = async (semaphore, value) =>
        {
            using var scope = await semaphore.LockAsync(50);
            await Task.Delay(100);
            value.Value++;

        };
    

        [TestMethod]
        [DataRow(150, false)]
        [DataRow(510, true)]
        public async Task LockAsync_Test(int expired,bool completed)
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var value = new LockValue { Value = 0 };
            var tasks = Task.WhenAll(_action(semaphore, value), _action(semaphore, value), _action(semaphore, value));
            await Task.WhenAny(tasks, Task.Delay(expired));
            if (completed)
            {
                value.Value.ShouldBe(3);
            }
            else { value.Value.ShouldBeLessThan(3); }
   
        }

        [TestMethod]
        [DataRow(120, false)]
        [DataRow(250, true)]
        public async Task LockAsync_TestTimeout(int expired, bool completed)
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var value = new LockValue { Value = 0 };
            var tasks = Task.WhenAll(_action2(semaphore, value), _action2(semaphore, value), _action2(semaphore, value));
            await Task.WhenAny(tasks, Task.Delay(expired));
            if (completed)
            {
                value.Value.ShouldBe(3);
            }
            else { value.Value.ShouldBeLessThan(3); }
        }
    }
}