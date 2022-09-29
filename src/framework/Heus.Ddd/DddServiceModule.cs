using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Heus.Ddd.JsonConverters;

namespace Heus.Ddd;

[DependsOn(typeof(CoreServiceModule))]
public class DddServiceModule : ServiceModuleBase, IPreConfigureServices
{
    public void PreConfigureServices(ServiceConfigurationContext context)
    {
      
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        JsonUtils.DefaultOptions.Converters.Add(new JsonEntityIdStringConverter());
    }

}