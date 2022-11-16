using Heus.Core.Uow;

namespace Heus.Core.DependencyInjection;

internal class ModuleHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleManager _moduleManager;
    public ModuleHostService(IServiceProvider serviceProvider, IModuleManager moduleContainer) {
        _serviceProvider = serviceProvider;
        _moduleManager = moduleContainer;
        }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var module in _moduleManager.Modules)
        {
            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var unitOfWorkManager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();
            var options = new UnitOfWorkOptions { IsTransactional = true,ServiceProvider = serviceProvider};

            using var unitOfWork = unitOfWorkManager.Begin(options);

            await module.Instance.InitializeAsync(_serviceProvider);

            await unitOfWork.CompleteAsync();
          
        }  
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
