using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Heus.Core.Utils;

public static class ThrowHelper
{
    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }
    [DoesNotReturn]
    internal static void Throw(string? paramName) =>
          throw new ArgumentNullException(paramName);

}
