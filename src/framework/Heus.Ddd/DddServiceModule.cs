using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Heus.Ddd.Data.Options;
using Heus.Ddd.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd;

public class DddServiceModule : ServiceModuleBase, IPreConfigureServices
{
    public void PreConfigureServices(ServiceConfigurationContext context)
    {
        JsonUtils.DefaultOptions.Converters.Add(new JsonEntityIdStringConverter());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ConfigureOptions<DbConnectionOptions>();
    }

}