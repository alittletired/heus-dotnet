

using Heus.Core.DependencyInjection;

namespace Heus.Settings;

public abstract class SettingValueProviderBase: ISettingValueProvider,IScopedDependency
{
    public abstract string Name { get; }

    protected ISettingStore SettingStore { get; }

    protected SettingValueProviderBase(ISettingStore settingStore)
    {
        SettingStore = settingStore;
    }

    public abstract Task<string?> GetOrNullAsync(SettingDefinition setting);

    public abstract Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings);
}
