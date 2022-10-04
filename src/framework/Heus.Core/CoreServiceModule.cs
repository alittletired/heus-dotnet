using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Core.ObjectMapping;
using Heus.Core.Security;
using Heus.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core;

public class CoreServiceModule : ServiceModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddObjectMapper();
        
        context.Services.Configure<JwtOptions>(context.Configuration.GetSection(JwtOptions.ConfigurationSection));
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
    }
}