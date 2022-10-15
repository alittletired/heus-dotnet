

using Heus.Core.DependencyInjection;

namespace Heus.Settings.Impl;

internal class SettingProvider : ISettingProvider, IScopedDependency
{
    public Task<List<SettingValue>> GetAllAsync(string[] names)
    {
        throw new NotImplementedException();
    }

    public Task<List<SettingValue>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetOrNullAsync(string name)
    {
        throw new NotImplementedException();
    }
}
