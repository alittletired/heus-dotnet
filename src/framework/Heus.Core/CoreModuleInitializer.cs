using System.Reflection;
using Heus.Core.Caching;
using Heus.Core.DependencyInjection;
using Heus.Core.ObjectMapping;
using Heus.Core.Security;


namespace Heus.Core;

public class CoreModuleInitializer : ModuleInitializerBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        
        // context.ServiceRegistrar.TypeScaning += (_,type) => {
        //     
        //     if (type.GetTypeInfo().GetCustomAttributes()
        //             .FirstOrDefault(t=>t.GetType().IsAssignableTo<IMapInfo>()) is IMapInfo mapInfo)
        //     {
        //         MapperHelper.AddObjectMap(type,mapInfo.MappingType, mapInfo.MapType);     
        //     }        
        // };
        
       
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddObjectMapper();
        
        context.Services.Configure<JwtOptions>(context.Configuration.GetSection(JwtOptions.ConfigurationSection));
        context.Services.AddHttpClient();
        context.Services.AddCache();
    }
}