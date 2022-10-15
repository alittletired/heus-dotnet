using System.Reflection;
using Mapster;

namespace Heus.Auth;

internal class AuthIDtoCodeGenerationRegister:ICodeGenerationRegister
{
    public void Register(CodeGenerationConfig config)
    {
        config.AdaptTo("[name]Dto",MapType.Projection|MapType.Map)
            .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "Heus.Auth.Entities");
        
        //config.GenerateMapper("[name]Mapper")
        //    .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "Heus.Auth.Entities");
        
        // config.AdaptFrom("[name]CreateDto")
        //     .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "Heus.Auth.Entities");
     
    }
}