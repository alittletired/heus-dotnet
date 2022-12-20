using Heus.Core.Utils;

namespace Heus.Core.Tests.Utils;

public class AsyncLocalUtilsTests
{
    private static AsyncLocal<int?> _asyncLocal = new();

    [Fact]
    public void BeginScope_Test()
    {
        using (AsyncLocalUtils.BeginScope(_asyncLocal, 1))
        {
            _asyncLocal.Value.ShouldBe(1);
            using (AsyncLocalUtils.BeginScope(_asyncLocal, 2))
            {
                _asyncLocal.Value.ShouldBe(2);
            }
            _asyncLocal.Value.ShouldBe(1);
        }
        _asyncLocal.Value.ShouldBeNull();

    }
}