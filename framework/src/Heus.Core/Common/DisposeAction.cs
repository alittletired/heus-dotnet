namespace Heus.Core;
/// <summary>
/// This class can be used to provide an action when
/// Dispose method is called.
/// </summary>
public class DisposeAction : IDisposable
{
    private readonly Action _action;
    private DisposeAction(Action action)
    {
        _action = action;
    }
    /// <summary>
    /// Creates a new <see cref="DisposeAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public static DisposeAction Create(Action action)
    {
        return new DisposeAction(action);
    }
    public void Dispose()
    {
        _action();
    }
}

