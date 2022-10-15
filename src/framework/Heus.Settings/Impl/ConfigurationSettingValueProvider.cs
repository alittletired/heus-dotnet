
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Heus.Settings.Impl
{
    internal class ConfigurationSettingValueProvider : ISettingValueProvider, IScopedDependency
    {
        public const string ConfigurationNamePrefix = "Settings:";
        public string Name => SettingValueProviderNames.Configuration;
        protected IConfiguration Configuration { get; }
        public ConfigurationSettingValueProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            return Task.FromResult(settings.Select(x => new SettingValue(x.Name, Configuration[ConfigurationNamePrefix + x.Name])).ToList());
        }
       
        public Task<string?> GetOrNullAsync(SettingDefinition setting)
        {
            var value = Configuration[ConfigurationNamePrefix + setting.Name];
            return Task.FromResult<string?>(value);
        }
    }
}
