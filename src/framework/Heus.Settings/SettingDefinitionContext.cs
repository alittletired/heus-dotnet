
using System.Collections.Immutable;


namespace Heus.Settings;

public class SettingDefinitionContext 
{
    protected Dictionary<string, SettingDefinition> Settings { get; }

    public SettingDefinitionContext(Dictionary<string, SettingDefinition> settings)
    {
        Settings = settings;
    }

    public virtual SettingDefinition? GetOrNull(string name)
    {
        return Settings.GetOrDefault(name);
    }

    public virtual IReadOnlyList<SettingDefinition> GetAll()
    {
        return Settings.Values.ToImmutableList();
    }

    public virtual void Add(params SettingDefinition[] definitions)
    {
        if (definitions.IsNullOrEmpty())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Settings[definition.Name] = definition;
        }
    }
}
