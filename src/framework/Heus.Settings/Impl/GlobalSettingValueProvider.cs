
namespace Heus.Settings.Impl
{
    internal class GlobalSettingValueProvider : SettingValueProviderBase
    {
     
        public GlobalSettingValueProvider(ISettingStore settingStore)
      : base(settingStore)
        {
        }
        public override string Name =>  SettingValueProviderNames.Global;
        public override Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            return SettingStore.GetAllAsync(settings.Select(x => x.Name).ToArray(), Name, null);
        }

        public override Task<string?> GetOrNullAsync(SettingDefinition setting)
        {
            return SettingStore.GetOrNullAsync(setting.Name, Name, null);
        }
    }
}
