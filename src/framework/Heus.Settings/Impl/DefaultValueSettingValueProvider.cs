
namespace Heus.Settings.Impl
{
    internal class DefaultValueSettingValueProvider : SettingValueProviderBase
    {

        public override string Name => SettingValueProviderNames.Default;

        public DefaultValueSettingValueProvider(ISettingStore settingStore)
            : base(settingStore)
        {

        }

        public override Task<string?> GetOrNullAsync(SettingDefinition setting)
        {
            return Task.FromResult(setting.DefaultValue);
        }

        public override Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            return Task.FromResult(settings.Select(x => new SettingValue(x.Name, x.DefaultValue)).ToList());
        }
    }
}
