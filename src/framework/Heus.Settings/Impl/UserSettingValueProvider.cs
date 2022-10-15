

using Heus.Core.Security;

namespace Heus.Settings.Impl;

internal class UserSettingValueProvider : SettingValueProviderBase
{
    public override string Name => SettingValueProviderNames.User;
    protected ICurrentUser CurrentUser { get; }
    public UserSettingValueProvider(ISettingStore settingStore, ICurrentUser currentUser)
        : base(settingStore)
    {
        CurrentUser = currentUser;
    }
    public override async Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
    {
        if (CurrentUser.Id == null)
        {
            return settings.Select(x => new SettingValue(x.Name, null)).ToList();
        }

        return await SettingStore.GetAllAsync(settings.Select(x => x.Name).ToArray(), Name, CurrentUser.Id.ToString());
    }

    public override async Task<string?> GetOrNullAsync(SettingDefinition setting)
    {
        if (CurrentUser.Id == null)
        {
            return null;
        }
        return await SettingStore.GetOrNullAsync(setting.Name, Name, CurrentUser.Id.ToString());
    }
}
