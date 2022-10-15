

namespace Heus.Settings;

internal interface ISettingProvider
{
    Task<string?> GetOrNullAsync( string name);

    Task<List<SettingValue>> GetAllAsync( string[] names);

    Task<List<SettingValue>> GetAllAsync();
}
