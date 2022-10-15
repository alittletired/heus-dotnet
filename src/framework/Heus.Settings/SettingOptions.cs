using Heus.Core;

namespace Heus.Settings;

public class SettingOptions
{
    public TypeList<ISettingDefinitionProvider> DefinitionProviders { get; } = new();

    public TypeList<ISettingValueProvider> ValueProviders { get; } = new();

}