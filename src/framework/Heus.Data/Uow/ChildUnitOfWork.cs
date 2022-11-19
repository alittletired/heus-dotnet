


namespace Heus.Data.Uow;
internal class ChildUnitOfWork : IUnitOfWork
{
    private readonly IUnitOfWork _parent;

    public UnitOfWorkOptions Options => _parent.Options;

    public IServiceProvider ServiceProvider => _parent.ServiceProvider;
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;

    public DbContext GetDbContext<TEntity>()
    {
        return _parent.GetDbContext<TEntity>();}
    public ChildUnitOfWork(IUnitOfWork parent)
    {
        _parent = parent;
        _parent.Disposed += (sender, args) => { Disposed?.Invoke(sender, args); };
    }

    public Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _parent.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {

    }
}