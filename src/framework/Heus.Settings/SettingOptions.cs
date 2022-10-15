using Heus.Core;

namespace Heus.Settings;

public class SettingOptions
{
    public TypeList<ISettingDefinitionProvider> DefinitionProviders { get; }

    public TypeList<ISettingValueProvider> ValueProviders { get; }

}