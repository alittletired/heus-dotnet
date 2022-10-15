using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Core.ObjectMapping;
using Heus.Core.Security;


namespace Heus.Core;

public class CoreModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddObjectMapper();
        
        context.Services.Configure<JwtOptions>(context.Configuration.GetSection(JwtOptions.ConfigurationSection));
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
    }
}